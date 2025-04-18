using System;
using System.Data.Common;

namespace QuizRuLet.Core.Models;

public class User
{
    private User(Guid id, string login, string password, List<Module> modules)    // LIST????
    {
        
    }


    public Guid Id {get; set;}
    
    public string Login {get; set;} = string.Empty;
    
    public string Password {get; set;} = string.Empty;
    
    public List<Module> Modules {get; set;} = [];
    
}