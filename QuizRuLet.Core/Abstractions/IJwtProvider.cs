using QuizRuLet.Core.Models;

namespace QuizRuLet.Core.Abstractions;

public interface IJwtProvider
{
    string GenerateToken(User user);
}
