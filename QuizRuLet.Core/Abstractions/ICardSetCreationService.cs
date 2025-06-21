using QuizRuLet.Core.Models;

namespace QuizRuLet.Core.Abstractions;

public interface ICardSetCreationService
{
    (List<Card>? Cards, string Error) Create(string inputData, string pairSeparator, string lineSeparator);
}
