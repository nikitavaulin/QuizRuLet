namespace QuizRuLet.DataAccess.Entities
{
    /// <summary>
    /// Сущность учебного модуля в БД
    /// </summary>
    public class ModuleEntity
    {
        public Guid Id {get; set;}
        public string Name {get; set;} = string.Empty;
        public string Description {get; set;} = string.Empty;
    
        // Навигационные поля
        public Guid UserId {get; set;}
        public UserEntity? User {get; set;} 
        public List<CardEntity> Cards {get; set;} = [];
    }
}
