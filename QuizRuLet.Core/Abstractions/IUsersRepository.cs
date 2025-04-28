using QuizRuLet.Core.Models;

namespace QuizRuLet.DataAccess.Repositories
{
    public interface IUsersRepository
    {
        Task Add(Guid id, string login, string password);
        Task Delete(Guid id);
        Task<List<User>> Get();
        Task<User?> GetById(Guid id);
        Task<List<User>> GetWithModules();
        Task Update(Guid id, string login, string password);
    }
}
