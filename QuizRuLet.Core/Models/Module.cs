using System;

namespace QuizRuLet.Core.Models;

public class Module
{
    public Guid Id {get; set;}
    
    public string Name {get; set;} = string.Empty;
    
    public string Description {get; set;} = string.Empty;
    
    public List<Card> Cards {get; set;} = [];
    
}
