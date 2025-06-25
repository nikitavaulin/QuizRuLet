using BCrypt.Net;
using QuizRuLet.Core.Abstractions;

namespace QuizRuLet.Infrastrucrture;

public class PasswordHasher : IPasswordHasher
{
    /// <summary>
    /// Хеширование пароля
    /// </summary>
    public string Generate(string password) =>
        BCrypt.Net.BCrypt.EnhancedHashPassword(password);

    /// <summary>
    /// Проверка пароля
    /// </summary>
    public bool Verify(string password, string hashedPassword) =>
        BCrypt.Net.BCrypt.EnhancedVerify(password, hashedPassword);     

}
