using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;

namespace QuizRuLet.Core.Models;

public class Module
{

    public const int MAX_MODULE_NAME_LENGTH = 50;
    public const int MAX_MODULE_DESCRIPTION_LENGTH = 350;
    
    
    private Module(Guid id, string name, string description, List<Card> cards)  // LIST????
    {
        Id = id;
        Name = name;
        Description = description;
        Cards = cards;
    }
    
    
    public Guid Id {get;}
    
    public string Name {get;} = string.Empty;
    
    public string Description {get;} = string.Empty;
    
    public List<Card> Cards {get;} = [];
    
    
    public static (Module Module, string Error) Create(Guid id, string name, string description, List<Card> cards)
    {
        var error = string.Empty; // сообщение об ошибке
        
        if (string.IsNullOrEmpty(name) || name.Length > MAX_MODULE_NAME_LENGTH)     // валидация названия
        {
            error = "Длина названия модуля должна быть от 3 до 50 символов";
        }
        
        if (description.Length > MAX_MODULE_DESCRIPTION_LENGTH)     // валидация описания
        {
            error = "Длина описания модуля не может быть больше 350 символов";
        }
        
        var module = new Module(id, name, description, cards);
        
        return (module, error);
    }
    
}
