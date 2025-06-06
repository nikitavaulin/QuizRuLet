using BCrypt.Net;
using QuizRuLet.Core.Abstractions;

namespace QuizRuLet.Infrastrucrture;

public class PasswordHasher : IPasswordHasher
{
    /// <summary>
    /// Хеширование пароля
    /// </summary>
    /// <param name="password">пароль</param>
    /// <returns>захешированный пароль</returns>
    public string Generate(string password) =>
        BCrypt.Net.BCrypt.EnhancedHashPassword(password);

    /// <summary>
    /// Проверка пароля
    /// </summary>
    /// <param name="password"></param>
    /// <param name="hashedPassword"></param>
    /// <returns></returns>
    public bool Verify(string password, string hashedPassword) =>
        BCrypt.Net.BCrypt.EnhancedVerify(password, hashedPassword);     

}
