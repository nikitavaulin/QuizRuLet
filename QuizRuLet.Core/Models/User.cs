using System;

namespace QuizRuLet.Core.Models;

public class User
{
    public Guid Id {get; set;}
    
    public string UserName {get; set;} = string.Empty;
    
    public string Password {get; set;} = string.Empty;
    
    public List<Modul> Moduls {get; set;} = [];
    
}