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
    private readonly IUsersRepository _userRepository;  // Репозиторий для работы с пользователями в БД
    private readonly IPasswordHasher _passwordHasher;   // Сервис для хэширования паролей
    private readonly IJwtProvider _jwtProvider;         // Провайдер для генерации JWT-токенов

    // Внедрение зависимостей
    public UserService(IUsersRepository userRepository, IPasswordHasher passwordHasher, IJwtProvider jwtProvider)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtProvider = jwtProvider;
    }

    // Получение всех пользователей в БД
    public async Task<List<User>> GetAllUsers()
    {
        return await _userRepository.Get();
    }

    // Получение пользователя по ID
    public async Task<User?> GetUserById(Guid id)
    {
        return await _userRepository.GetById(id);
    }
    
    // Получение пользователя по логину
    public async Task<User?> GetUserByName(string login)
    {
        return await _userRepository.GetByLogin(login);
    }

    // Создание нового пользователя
    public async Task<Guid> CreateUser(User user)
    {
        return await _userRepository.Create(user);
    }

    // Обновление полей пользователя по ID
    public async Task<Guid> UpdateUser(Guid id, string login, string password)
    {
        return await _userRepository.Update(id, login, password);
    }

    // Удаление пользователя по ID
    public async Task<Guid> DeleteUser(Guid id)
    {
        return await _userRepository.Delete(id);
    }

    // Регистрация нового пользователя
    public async Task<(bool Success, string Error)> Register(string login, string password)   
    {
        var error = string.Empty; // сообщение об ошибке
        var result = false; // флаг успешности операции
        
        // проверка существует ли пользователь с таким логином
        var isUserExist = await _userRepository.IsUserLoginExist(login);
    
        if (isUserExist) // уже существует
        {
            error = "Пользователь с таким логином уже зарегистрирован";
        }
        else if (string.IsNullOrEmpty(password) // валидация пароля
            || password.Length < User.MIN_PASSWORD_LENGTH     
            || password.Length > User.MAX_PASSWORD_LENGTH)     
        {
            error = $"Длина пароля должна быть от {User.MIN_PASSWORD_LENGTH} до {User.MAX_PASSWORD_LENGTH} символов"; 
        }
        else    
        {
            // хэширование пароля 
            var hashedPassword = _passwordHasher.Generate(password);

            // создание нового пользователя
            var userCreationResult = User.Create(Guid.NewGuid(), login, hashedPassword);
            error = userCreationResult.Error; // ошибка при создании пользователя
            
            if (string.IsNullOrEmpty(error)) // проверка ошибки
            {
                await _userRepository.Create(userCreationResult.User); // сохранение пользователя в БД
                result = true; 
            }
        }

        return (result, error);
    }

    // Авторизация пользователя
    public async Task<(string Token, string Error)> Login(string login, string password)
    {
        var token = ""; // jwt токен
        var error = ""; // сообщение об ошибке
        
        // поиск пользователя по логину
        var user = await _userRepository.GetByLogin(login); 
        
        if (user is null) // пользователь не найден
        {
            error = "Пользователь не зарегистрирован в системе";
        }
        else
        {
            var result = _passwordHasher.Verify(password, user.PasswordHash);   // верификация пароля
        
            if (result == false) // пароль неверный
            {
                error = "Неверный пароль";
            }
            else                                                                // успешный вход
            {
                // генерация JWT-токен для авторизованного пользователя
                token = _jwtProvider.GenerateToken(user);   
            }
        }
        
        return (token, error);
    }
}