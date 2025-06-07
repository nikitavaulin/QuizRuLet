namespace QuizRuLet.API.Contracts
{
    public record ModuleWithCardsResponse
    (
        Guid Id,
        string Name,
        string Description,
        int Progres,
        List<CardResponse> Cards
    );
        
    
}
