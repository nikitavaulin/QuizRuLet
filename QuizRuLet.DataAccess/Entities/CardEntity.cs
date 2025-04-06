namespace QuizRuLet.DataAccess.Entities;

public class CardEntity
{
    public Guid Id {get; set;}
    
    public string FrontSide {get; set;} = string.Empty;
    
    public string BackSide {get; set;} = string.Empty;
    
    public bool IsLearned {get; set;}
}
