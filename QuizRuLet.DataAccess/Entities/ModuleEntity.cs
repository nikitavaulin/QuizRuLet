namespace QuizRuLet.DataAccess.Entities;

public class ModuleEntity
{
    public Guid Id {get; set;}
    
    public string Name {get; set;} = string.Empty;
    
    public string Description {get; set;} = string.Empty;
    
    public List<CardEntity> Cards {get; set;} = [];
    
    public Guid UserId {get; set;}
    
    public UserEntity? User {get; set;} 
}
