using QuizRuLet.Core.Models;

namespace QuizRuLet.Core.Abstractions;

public interface ICardService
{
    Task<Guid> CreateCard(Card card, Guid moduleId);
    Task<Guid> DeleteCard(Guid id);
    Task<List<Card>> GetAllCards();
    Task<Card?> GetCardById(Guid id);
    Task<List<Card>> GetCardsByLearningFlag(Guid moduleId, bool isLearned);
    Task<List<Card>> GetCardsByModule(Guid moduleId);
    Task<Guid> UpdateCard(Guid id, string frontSide, string backSide, bool isLearned, Guid moduleId);
}
