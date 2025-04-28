using QuizRuLet.Core.Models;

namespace QuizRuLet.DataAccess.Repositories
{
    public interface ICardsRepository
    {
        Task Add(Guid id, string frontSide, string backSide, bool isLearned, Guid moduleId);
        Task Delete(Guid id);
        Task<List<Card>> Get();
        Task<Card?> GetById(Guid id);
        Task<List<Card>> GetByLearningFlag(Guid moduleId, bool isLearned);
        Task<List<Card>> GetByModule(Guid moduleId);
        Task Update(Guid id, string frontSide, string backSide, bool isLearned, Guid moduleId);
    }
}
