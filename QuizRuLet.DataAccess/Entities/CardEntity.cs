namespace QuizRuLet.DataAccess.Entities
{
    /// <summary>
    /// Сущность флеш-карточки в БД
    /// </summary>
    public class CardEntity
    {
        public Guid Id {get; set;}
        public string FrontSide {get; set;} = string.Empty;
        public string BackSide {get; set;} = string.Empty;
        public bool IsLearned {get; set;}
    
        // Навигационные поля
        public Guid ModuleId {get; set;}
        public ModuleEntity? Module {get; set;}
    
    }
}
