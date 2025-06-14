using QuizRuLet.Core.Models;

namespace QuizRuLet.Core.Abstractions
{
    public interface ICardsRepository
    {
        Task<Guid> Add(Guid id, string frontSide, string backSide, bool isLearned, Guid moduleId);
        Task<Guid> Create(Card card, Guid moduleId);
        Task<Guid> Delete(Guid id);
        Task<Guid> DeleteByModule(Guid moduleId);
        Task<List<Card>> Get();
        Task<Card?> GetById(Guid id);
        Task<List<Card>> GetByLearningFlag(Guid moduleId, bool isLearned);
        Task<List<Card>> GetByModule(Guid moduleId);        
        Task<int> GetCountCardsInModule(Guid moduleId);
        Task<int> GetCountCardsByLearningFlagInModule(Guid moduleId, bool isLearned);
        Task<Guid> Update(Guid id, string frontSide, string backSide, bool isLearned, Guid moduleId);
        Task<Guid> UpdateLearningFlag(Guid cardId, bool isLearned);
        Task<Guid> UpdateLearningFlagInModule(Guid moduleId, bool isLearned);
        Task<Guid> UpdatePartly(Guid id, string frontSide, string backSide);
    }
}
