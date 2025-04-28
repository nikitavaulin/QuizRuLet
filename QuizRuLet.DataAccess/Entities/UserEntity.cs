using System;
using QuizRuLet.Core.Models;

namespace QuizRuLet.DataAccess.Entities;

public class UserEntity : User
{
    public List<ModuleEntity> Modules {get; set;} = [];     // модули пользователя
    
            
}
