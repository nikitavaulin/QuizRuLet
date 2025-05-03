using QuizRuLet.Core.Models;

namespace QuizRuLet.Core.Abstractions;

public interface ICardSetCreationService
{
    List<Card> Create(string inputData, string pairSeparator, string lineSeparator);
}
