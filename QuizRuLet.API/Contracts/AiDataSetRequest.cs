namespace QuizRuLet.API.Contracts
{
    public record AiDataSetRequest
    (
        string Data,
        int CardsCount
        // Guid ModuleId
    );
}
