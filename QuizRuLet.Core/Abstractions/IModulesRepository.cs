

namespace QuizRuLet.DataAccess.Repositories     // FIXME?
{
    public interface IModulesRepository
    {
        Task Add(Guid id, string name, string description, Guid userId);
        Task Delete(Guid id);
        Task<List<Core.Models.Module>> Get();
        Task<Core.Models.Module?> GetById(Guid id);
        Task<List<Core.Models.Module>> GetByUser(Guid userId);
        Task Update(Guid id, string name, string description, Guid userId);
    }
}
