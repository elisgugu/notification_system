using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    IConfiguration _configuration;
    public JwtMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _configuration = configuration;
    }

    public async Task Invoke(HttpContext context)
    {
        // Check if the request has an authorization header
        if (context.Request.Headers.TryGetValue("Authorization", out var authHeader))
        {
            var token = authHeader.ToString().Split(" ").LastOrDefault();

            // Validate and decode the JWT token
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
              
                ValidateIssuerSigningKey = true, // Set to true if you want to validate the signing key
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Jwt:Key"])), // Provide your own signing key
                ValidateIssuer = true, // Set to true if you want to validate the issuer
                ValidIssuer = _configuration["Jwt:Issuer"], // Provide your own issuer
                ValidateAudience = true, // Set to true if you want to validate the audience
                ValidAudience = _configuration["Jwt:Audience"], // Provide your own audience
            };

            try
            {
                // Validate the token and extract claims
                var claimsPrincipal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);

                // Set the user claims in the HttpContext
                context.User = claimsPrincipal;
            }
            catch (SecurityTokenValidationException)
            {
                // Handle token validation errors, e.g., token expired or invalid signature
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }
        }

        // Call the next middleware
        await _next(context);
    }
}

