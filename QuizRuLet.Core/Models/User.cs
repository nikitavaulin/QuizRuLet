using System;
using System.Data.Common;
using System.Runtime.InteropServices;

namespace QuizRuLet.Core.Models;

public class User
{
    public const int MAX_USERLOGIN_LENGTH = 30;
    public const int MIN_USERLOGIN_LENGTH = 3;
    public const int MAX_PASSWORD_LENGTH = 50;
    public const int MIN_PASSWORD_LENGTH = 6;


    public Guid Id { get; set; }
    public string Login { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public List<Module> Modules { get; set; } = [];


    private User(Guid id, string login, string password)
    {
        Id = id;
        Login = login;
        PasswordHash = password;
    }
    
    public static (User User, string Error) Create(Guid id, string login, string password)
    {
        var error = string.Empty;

        if (string.IsNullOrEmpty(login)
            || login.Length < MIN_USERLOGIN_LENGTH     // валидация названия
            || login.Length > MAX_USERLOGIN_LENGTH)     // валидация названия
        {
            error = $"Длина логина должна быть от {MIN_USERLOGIN_LENGTH} до {MAX_USERLOGIN_LENGTH} символов";
        }

        if (string.IsNullOrEmpty(password)
            || password.Length < MIN_PASSWORD_LENGTH     // валидация названия
            || password.Length > MAX_PASSWORD_LENGTH)     // валидация названия
        {
            error = $"Длина пароля должна быть от {MIN_PASSWORD_LENGTH} до {MAX_PASSWORD_LENGTH} символов";
        }

        var user = new User(id, login, password);

        return (user, error);
    }

}