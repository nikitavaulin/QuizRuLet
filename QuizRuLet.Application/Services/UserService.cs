using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data.Common;
using System.Reflection.Metadata.Ecma335;
using Microsoft.VisualBasic;
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

    public async Task<(bool Success, string Error)> Register(string login, string password)   // TODO
    {
        var error = string.Empty;
        var result = false;
            
        var isUserExist = await _userRepository.IsUserLoginExist(login);
    
        if (isUserExist)                                                                     // есть ли такой юзер
        {
            error = "Пользователь с таким логином уже зарегистрирован";
        }
        else if (string.IsNullOrEmpty(password)                                              // валидация пароля
            || password.Length < User.MIN_PASSWORD_LENGTH     
            || password.Length > User.MAX_PASSWORD_LENGTH)     
        {
            error = $"Длина пароля должна быть от {User.MIN_PASSWORD_LENGTH} до {User.MAX_PASSWORD_LENGTH} символов"; 
        }
        else    
        {
            var hashedPassword = _passwordHasher.Generate(password);

            var userCreationResult = User.Create(Guid.NewGuid(), login, hashedPassword);
            error = userCreationResult.Error;
            
            if (string.IsNullOrEmpty(error))                                 // если всё успешно
            {
                await _userRepository.Create(userCreationResult.User);
                result = true;
            }
        }

        return (result, error);

    }

    public async Task<(string Token, string Error)> Login(string login, string password)
    {
        var token = "";
        var error = "";
        
        var user = await _userRepository.GetByLogin(login); 
        
        if (user is null)
        {
            error = "Пользователь не зарегистрирован в системе";
        }
        else
        {
            var result = _passwordHasher.Verify(password, user.PasswordHash);   // верификация пароля
        
            if (result == false)    // неверный пароль
            {
                error = "Неверный пароль";
            }
            else
            {
                token = _jwtProvider.GenerateToken(user);   // создание токена, если всё успешно
            }
        }
        
        return (token, error);
    }
}
