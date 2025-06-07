namespace QuizRuLet.API.Contracts
{
    public record DataSetRequest
    (
        string Data,
        string LineSeparator,
        string PairSeparator
        // Guid ModuleId
    );
}
