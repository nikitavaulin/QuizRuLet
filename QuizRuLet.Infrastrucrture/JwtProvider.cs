using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using QuizRuLet.Core.Abstractions;
using QuizRuLet.Core.Models;

namespace QuizRuLet.Infrastrucrture;

public class JwtProvider : IJwtProvider
{
    private readonly JwtOptions _options;   // параметры для токена

    /// Внедрение зависимостей
    public JwtProvider(IOptions<JwtOptions> options)
    {
        _options = options.Value;
    }

    // Генерация JWT-токена
    public string GenerateToken(User user)
    {
        // клеймы для пейлоада токена
        Claim[] claims = [
            new("userId", user.Id.ToString()),      // ID пользователя
            new(ClaimTypes.Role, "User")            // Роль пользователь
            ];

        // ключ подписи (на основе секретного ключа)
        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)),
            SecurityAlgorithms.HmacSha256);

        // генерация токена
        var token = new JwtSecurityToken(
            claims: claims,
            signingCredentials: signingCredentials,
            expires: DateTime.UtcNow.AddHours(_options.ExpiredHours)    // время истечения срока действия токена
        );

        // преобразование объекта в строку
        var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

        return tokenValue;
    }
}
