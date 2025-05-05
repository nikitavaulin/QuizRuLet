namespace QuizRuLet.Core.Abstractions;

public interface IPasswordHasher
{
    string Generate(string password);
}
