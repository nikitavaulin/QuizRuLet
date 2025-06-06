using QuizRuLet.Core.Models;

namespace QuizRuLet.Core.Abstractions;

public interface IUserService
{
    Task<Guid> CreateUser(User user);
    Task<Guid> DeleteUser(Guid id);
    Task<List<User>> GetAllUsers();
    Task<User> GetUserById(Guid id);
    Task Register(string login, string password);
    Task<Guid> UpdateUser(Guid id, string login, string password);
    Task<string> Login(string login, string password);
}
