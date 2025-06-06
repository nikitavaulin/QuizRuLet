namespace QuizRuLet.API.Contracts
{
    public record ModuleWithCardsResponse
    (
        Guid Id,
        string Name,
        string Description,
        List<CardResponse> Cards
    );
        
    
}
