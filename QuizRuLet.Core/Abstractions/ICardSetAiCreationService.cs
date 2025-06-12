using QuizRuLet.Core.Models;

namespace QuizRuLet.Core.Abstractions;

public interface ICardSetAiCreationService
{
    Task<(List<Card>? Cards, string Error)> Create(string data, int countCards);
}
