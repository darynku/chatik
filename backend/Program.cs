using DarynChat.API.BaseChatHub;
using DarynChat.API.DatabaseContext;
using DarynChat.API.Extension;
using DarynChat.API.JWT;
using Microsoft.EntityFrameworkCore;

namespace DarynChat.API;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddControllers();
        
        builder.Services.AddAuth(builder.Configuration);
        builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions.Jwt)));

        builder.Services.AddDbContext<ChatDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

        builder.Services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = builder.Configuration.GetConnectionString("Cache");
        });

        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.WithOrigins("http://localhost:5173")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });
        
        builder.Services.AddSignalR();
        var app = builder.Build();
        
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();

        }
        app.UseAuthentication();
        app.UseAuthorization();
        
        app.MapControllers();

        app.UseCors();
        app.MapHub<ChatHub>("/chat");
        app.Run();
    }
}