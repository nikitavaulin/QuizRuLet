namespace QuizRuLet.Core.Abstractions;

public interface IAiApiProvider
{
    static abstract Task<(string Response, string Error)> SendRequest(string prompt);
}
