using Microsoft.EntityFrameworkCore;
using common_data.Data;
using common_data.Interfaces;
using notification_center_api.Repository;
using common_data.Models;
using Microsoft.AspNetCore.Mvc;
using notification_center_api.Hubs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace notification_center_api
{
    public class Program {
       


        public static void Main(string[] args) {
            var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  policy =>
                                  {
                                      policy.WithOrigins("http://example.com",
                                                          "http://www.contoso.com");
                                  });
            });

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey
                    (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true
                };
            });


            builder.Services.AddDbContext<NotificationCenterContext>(conn => conn.UseSqlServer("Data Source=.\\Database\\notification_center.mdf"));
            // Add services to the container.
            builder.Services.AddCors();
            builder.Services.AddSignalR();
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            builder.Services.AddScoped<IUserLogin, UserLoginService>();
            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();


            builder.Services.AddScoped<IRequestRepository, RequestRepository>();

            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment()) {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseMiddleware<JwtMiddleware>();
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors(x => x
    .AllowAnyMethod() //AlowAllMethods
    .AllowAnyHeader()
    .WithOrigins("https://localhost:7175", "http://localhost:5292")

    .AllowCredentials()
);
          //  app.MapHub<RequestHub>("/requestMessages");
            app.MapHub<CertificateHub>("/certificateMessages");

            app.MapGet("api/Requests/", 
                (IRequestRepository reqRepo) => 
                Results.Ok(reqRepo.GetAllRequests())).RequireAuthorization();

            app.MapPost("api/Login/",
                [AllowAnonymous]
                async ([FromBody]User user, IUserLogin userLogin) => {

                var userToLogin = await userLogin.AuthenticateUser(user.UserName, user.Password);
               if (userToLogin.Id != string.Empty)
                    {
                        var issuer = builder.Configuration["Jwt:Issuer"];
                        var audience = builder.Configuration["Jwt:Audience"];
                        var key = Encoding.ASCII.GetBytes
                        (builder.Configuration["Jwt:Key"]);
                        var tokenDescriptor = new SecurityTokenDescriptor
                        {
                            Subject = new ClaimsIdentity(new[]
                            {
                                new Claim("Id", userToLogin.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti,
                Guid.NewGuid().ToString())
             }),
                            Expires = DateTime.UtcNow.AddMinutes(50),
                            Issuer = issuer,
                            Audience = audience,
                            SigningCredentials = new SigningCredentials
                            (new SymmetricSecurityKey(key),
                            SecurityAlgorithms.HmacSha512Signature)
                        };
                        var tokenHandler = new JwtSecurityTokenHandler();
                        var token = tokenHandler.CreateToken(tokenDescriptor);
                        var jwtToken = tokenHandler.WriteToken(token);
                        var stringToken = tokenHandler.WriteToken(token);
                        return Results.Ok(stringToken);
                    }

                    return Results.Unauthorized();
                });
     


            app.Run();
        }
    }
}