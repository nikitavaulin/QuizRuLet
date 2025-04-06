using System;

namespace QuizRuLet.DataAccess.Entities;

public class UserEntity
{
    public Guid Id {get; set;}
    
    public string Login {get; set;} = string.Empty;
    
    public string Password {get; set;} = string.Empty;
    
    public List<ModuleEntity> Modules {get; set;} = [];
    
    
    
}
