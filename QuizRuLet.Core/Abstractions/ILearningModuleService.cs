using QuizRuLet.Core.Models;


namespace QuizRuLet.Core.Abstractions;

public interface ILearningModuleService
{
    Task<List<Card>> GetAllCards(Guid moduleId);
    Task<List<Card>> GetCards(Guid moduleId, bool isLearned);
    Task<Guid> UpdateLearningFlag(Guid cardId, bool isLearned);
    Task<Guid> UpdateLearningFlagInModule(Guid moduleId, bool isLearned);
}
