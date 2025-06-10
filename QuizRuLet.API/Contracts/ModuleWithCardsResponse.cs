namespace QuizRuLet.API.Contracts
{
    public record ModuleWithCardsResponse
    (
        Guid Id,
        string Name,
        string Description,
        int Progress,
        int CountCards,
        int CountLearned,
        List<CardResponse> Cards
    );
        
    
}
