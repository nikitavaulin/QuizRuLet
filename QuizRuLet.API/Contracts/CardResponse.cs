namespace QuizRuLet.API.Contracts
{
    public record CardResponse
    (
        string FrontSide,
        string BackSide,
        bool IsLearned
    );
        
    
}
