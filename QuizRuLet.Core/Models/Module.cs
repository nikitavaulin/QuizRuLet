using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;

namespace QuizRuLet.Core.Models
{
    /// <summary>
    /// Класс доменной области: Учебный модуль
    /// </summary>
    public class Module
    {

        // константы для валидации
        public const int MAX_MODULE_NAME_LENGTH = 50;
        public const int MIN_MODULE_NAME_LENGTH = 3;
        public const int MAX_MODULE_DESCRIPTION_LENGTH = 350;

        // поля
        public Guid Id {get;}                                   // Уникальный ID
        public string Name {get;} = string.Empty;               // Название модуля
        public string Description {get;} = string.Empty;        // Описание модуля    
    
        private Module(Guid id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }
    
        // Создание модуля
        public static (Module Module, string Error) Create(Guid id, string name, string description)
        {
            // сообщение об ошибке
            var error = string.Empty;                               

            // валидация названия
            if (string.IsNullOrEmpty(name)                              
                || name.Length < MIN_MODULE_NAME_LENGTH
                || name.Length > MAX_MODULE_NAME_LENGTH)     
            {
                error = $"Длина названия модуля должна быть от {MIN_MODULE_NAME_LENGTH} до {MAX_MODULE_NAME_LENGTH} символов";
            }

            // валидация описания
            if (description.Length > MAX_MODULE_DESCRIPTION_LENGTH)     
            {
                error = $"Длина описания модуля не может быть больше {MAX_MODULE_DESCRIPTION_LENGTH} символов";
            }

            // создание объекта
            var module = new Module(id, name, description);
        
            return (module, error);
        }
    }
}
