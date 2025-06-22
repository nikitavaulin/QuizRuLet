using System;
using System.Data.Common;
using System.Runtime.InteropServices;

namespace QuizRuLet.Core.Models
{
    /// <summary>
    /// Класс доменной области: Пользователь
    /// </summary>
    public class User
    {
        // константы для валидации
        public const int MAX_USERLOGIN_LENGTH = 30;
        public const int MIN_USERLOGIN_LENGTH = 3;
        public const int MAX_PASSWORD_LENGTH = 50;
        public const int MIN_PASSWORD_LENGTH = 6;

        // поля
        public Guid Id { get; set; }                                        // Уникальный ID
        public string Login { get; set; } = string.Empty;                   // Логин (username пользователя)
        public string PasswordHash { get; private set; } = string.Empty;    // Хэшированный пароль

        private User(Guid id, string login, string passwordHash)
        {
            Id = id;
            Login = login;
            PasswordHash = passwordHash;
        }

        // Создание пользователя    
        public static (User User, string Error) Create(Guid id, string login, string passwordHash)
        {
            // сообщение об ошибке
            var error = string.Empty;

            // валидация названия
            if (string.IsNullOrEmpty(login)                 
                || login.Length < MIN_USERLOGIN_LENGTH     
                || login.Length > MAX_USERLOGIN_LENGTH)
            {
                error = $"Длина логина должна быть от {MIN_USERLOGIN_LENGTH} до {MAX_USERLOGIN_LENGTH} символов";
            }

            // создание объекта
            var user = new User(id, login, passwordHash);

            return (user, error);
        }

    }
}