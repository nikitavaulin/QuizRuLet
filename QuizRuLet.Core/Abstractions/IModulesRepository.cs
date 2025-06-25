

namespace QuizRuLet.Core.Abstractions
{
    public interface IModulesRepository
    {
        Task<Guid> Create(Models.Module module, Guid moduleId);
        Task<Guid> Delete(Guid id);
        Task<List<Models.Module>> Get();
        Task<Models.Module?> GetById(Guid id);
        Task<List<Models.Module>> GetByUser(Guid userId);
        Task<Guid> Update(Guid id, string name, string description);
        Task<Guid> UpdateName(Guid id, string name);
        Task<Guid> UpdateDescription(Guid id, string description);
    }
}
