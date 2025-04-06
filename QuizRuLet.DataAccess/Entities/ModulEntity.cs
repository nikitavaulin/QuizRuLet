namespace QuizRuLet.DataAccess.Entities;

public class ModulEntity
{
    public Guid Id {get; set;}
    
    public string Name {get; set;} = string.Empty;
    
    public string Description {get; set;} = string.Empty;
    
    public List<CardEntity> Cards {get; set;} = [];
}
