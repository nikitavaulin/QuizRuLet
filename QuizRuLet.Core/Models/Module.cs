using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;

namespace QuizRuLet.Core.Models;

public class Module
{

    public const int MAX_MODULE_NAME_LENGTH = 50;
    public const int MIN_MODULE_NAME_LENGTH = 3;
    public const int MAX_MODULE_DESCRIPTION_LENGTH = 350;
    
    
    private Module(Guid id, string name, string description)
    {
        Id = id;
        Name = name;
        Description = description;
    }
    
    
    public Guid Id {get;}
    
    public string Name {get;} = string.Empty;
    
    public string Description {get;} = string.Empty;
    
    public List<Card> Cards {get;} = [];
    
    
    public static (Module Module, string Error) Create(Guid id, string name, string description)
    {
        var error = string.Empty; // сообщение об ошибке
        
        if (string.IsNullOrEmpty(name) 
            || name.Length > MIN_MODULE_NAME_LENGTH
            || name.Length > MAX_MODULE_NAME_LENGTH)     // валидация названия
        {
            error = $"Длина названия модуля должна быть от {MIN_MODULE_NAME_LENGTH} до {MAX_MODULE_NAME_LENGTH} символов";
        }
        
        if (description.Length > MAX_MODULE_DESCRIPTION_LENGTH)     // валидация описания
        {
            error = $"Длина описания модуля не может быть больше {MAX_MODULE_DESCRIPTION_LENGTH} символов";
        }
        
        // ВАЛИДАЦИЯ СПИСКА CARDS?
        
        var module = new Module(id, name, description);
        
        return (module, error);
    }
    
}
