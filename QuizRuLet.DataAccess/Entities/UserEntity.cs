using System;
using QuizRuLet.Core.Models;

namespace QuizRuLet.DataAccess.Entities
{
    /// <summary>
    /// Сущность пользователя в БД
    /// </summary>
    public class UserEntity
    {
        public Guid Id {get; set;}
        public string Login {get; set;} = string.Empty;
        public string PasswordHash {get; set;} = string.Empty;
    
        // Навигационные поля
        public List<ModuleEntity> Modules {get; set;} = [];
    }
}
