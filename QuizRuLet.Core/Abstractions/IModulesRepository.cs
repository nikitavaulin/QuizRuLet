

namespace QuizRuLet.Core.Abstractions
{
    public interface IModulesRepository
    {
        Task<Guid> Add(Guid id, string name, string description, Guid userId);
        Task<Guid> Create(Models.Module module, Guid moduleId);
        Task<Guid> Delete(Guid id);
        Task<List<Models.Module>> Get();
        Task<Models.Module?> GetById(Guid id);
        Task<List<Models.Module>> GetByUser(Guid userId);
        Task<Guid> Update(Guid id, string name, string description, Guid userId);
    }
}
