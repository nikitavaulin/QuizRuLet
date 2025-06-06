using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data.Common;
using System.Reflection.Metadata.Ecma335;
using QuizRuLet.Core.Abstractions;
using QuizRuLet.Core.Models;

namespace QuizRuLet.Application.Services;

public class UserService : IUserService
{
    private readonly IUsersRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtProvider _jwtProvider;

    public UserService(IUsersRepository userRepository, IPasswordHasher passwordHasher, IJwtProvider jwtProvider)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtProvider = jwtProvider;
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

    public async Task Register(string login, string password)   // TODO
    {
        // VALIDATION!
    
        var hashedPassword = _passwordHasher.Generate(password);

        var user = User.Create(Guid.NewGuid(), login, hashedPassword).User;

        await _userRepository.Create(user);
    }

    public async Task<string> Login(string login, string password)
    {
        var user = await _userRepository.GetByLogin(login);         // МОЖЕТ БЫТЬ NULL - FIX

        var result = _passwordHasher.Verify(password, user.PasswordHash);   // верификация пароля
        
        if (result == false)
        {
            throw new Exception("неправильный пароль");             // FIX
        }

        var token = _jwtProvider.GenerateToken(user);
        
        return token;
    }
}
