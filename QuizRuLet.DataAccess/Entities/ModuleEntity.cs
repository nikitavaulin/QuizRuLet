namespace QuizRuLet.DataAccess.Entities;

public class ModuleEntity : Core.Models.Module
{    
    public List<CardEntity> Cards {get; set;} = [];
    
    public Guid UserId {get; set;}
    
    public UserEntity? User {get; set;} 
}
