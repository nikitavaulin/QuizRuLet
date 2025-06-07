namespace QuizRuLet.API.Contracts
{
    public record CardCreateRequest
    (
        string FrontSide,
        string BackSide
        // Guid ModuleId
    );
}
