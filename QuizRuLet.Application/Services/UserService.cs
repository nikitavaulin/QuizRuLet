using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data.Common;
using System.Reflection.Metadata.Ecma335;
using QuizRuLet.Core.Abstractions;
using QuizRuLet.Core.Models;

namespace QuizRuLet.Application.Services;

public class UserService
{
    private readonly IUsersRepository _userRepository;

    public UserService(IUsersRepository userRepository)
    {
        _userRepository = userRepository; 
    }
    
    
    public async Task<List<User>> GetAllUsers()
    {
        return await _userRepository.Get();
    }
    
    public async Task<User> GetUserById(Guid id)
    {
        return await _userRepository.GetById(id);           // TODO Validation
    }
    
    public async Task<Guid> CreateUser(User user)
    {
        return await _userRepository.Create(user);
    }
    
    public async Task<Guid> UpdateUser(Guid id, string login, string password)
    {
        return await _userRepository.Update(id, login, password);
    }
    
    public async Task<Guid> DeleteUser(Guid id)
    {
        return await _userRepository.Delete(id);
    }
    
}
