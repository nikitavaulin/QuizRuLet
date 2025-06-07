namespace QuizRuLet.API.Contracts
{
    public record CardResponse
    (
        Guid Id,
        string FrontSide,
        string BackSide,
        bool IsLearned
    );
        
    
}
