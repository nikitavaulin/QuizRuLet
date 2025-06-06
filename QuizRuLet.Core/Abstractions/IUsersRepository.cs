using QuizRuLet.Core.Models;

namespace QuizRuLet.Core.Abstractions
{
    public interface IUsersRepository
    {
        Task<Guid> Add(Guid id, string login, string password);
        Task<Guid> Create(User user);
        Task<Guid> Delete(Guid id);
        Task<List<User>> Get();
        Task<User?> GetById(Guid id);
        Task<User?> GetByLogin(string login);
        Task<List<User>> GetWithModules();
        Task<Guid> Update(Guid id, string login, string password);
        Task<bool> IsUserLoginExist(string login);
    }
}
