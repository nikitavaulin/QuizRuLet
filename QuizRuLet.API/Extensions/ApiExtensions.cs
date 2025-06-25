using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using QuizRuLet.Infrastrucrture;

namespace QuizRuLet.API.Extensions;

/// <summary>
/// Класс расширения для добавления аутентификации и авторизации в DI контейнер
/// </summary>
public static class ApiExtensions
{
    public static void AddApiAuthentification(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        var jwtOptions = configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>();
        
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>            // настройка схемы аутентификации
            {
                options.TokenValidationParameters = new()                               // валидация параметров токена
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey))
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies["tasty-cookies"];        // обработка события: отправка токена в куки
                        return Task.CompletedTask;
                    }
                };
            });

        services.AddAuthorization();
    }
}
