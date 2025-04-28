using QuizRuLet.Core.Models;

namespace QuizRuLet.DataAccess.Entities;

public class CardEntity : Card
{
    public Guid ModuleId {get; set;}
    
    public ModuleEntity? Module {get; set;}
    
}
