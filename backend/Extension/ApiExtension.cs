using System.Text;
using DarynChat.API.JWT;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace DarynChat.API.Extension;

public static class ApiExtension
{
    public static void AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IJwtService, JwtService>();
        

        
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
        {
            var key = configuration.GetSection(JwtOptions.Jwt).Get<JwtOptions>()
                      ?? throw new ApplicationException("Wrong configuration");

            var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key.SecretKey));

            options.TokenValidationParameters = new()
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = symmetricKey
            };

            options.Events = new JwtBearerEvents()
            {
                OnMessageReceived = context =>
                {
                    context.Token = context.Request.Cookies["cookie"];
                    return Task.CompletedTask;
                }
            };
        });
        services.AddAuthorization();
    }
}