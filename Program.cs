using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using notification_system.Hubs;
using notification_system.MiddlewareExtensions;
using notification_system.Models;
using notification_system.Repository;
using notification_system.Services;
using notification_system.SubscribeTableDependencies;

namespace notification_system {
    public class Program {
        public static void Main(string[] args) {
            var builder = WebApplication.CreateBuilder(args);
           
            builder.Services.AddDbContext<NotificationCenterContext>(conn => conn.UseSqlServer("Data Source=.\\Database\\notification_center.mdf"));
            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<NotificationCenterContext>();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddControllersWithViews();
            builder.Services.AddSignalR();
            builder.Services.AddScoped<IUserLogin, UserLoginService>();
            builder.Services.AddScoped<IRequestRepository, RequestRepository>();
            builder.Services.AddScoped<ICertificateRepository, CertificateRepository>();
            builder.Services.AddSingleton<RequestHub>();
            builder.Services.AddSingleton<CertificateHub>();
            builder.Services.AddSingleton<SubscribeRequestTableDependency>();
            builder.Services.AddScoped<ExpirationService>();
           

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment()) {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
          
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
      
      
            app.MapHub<RequestHub>("/requestMessages");
            app.MapHub<CertificateHub>("/certificateMessages");
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");


            app.UseSqlTableDependency<SubscribeRequestTableDependency>(app.Configuration.GetConnectionString("DefaultConnection"));
            app.Run();
        }
    }
}