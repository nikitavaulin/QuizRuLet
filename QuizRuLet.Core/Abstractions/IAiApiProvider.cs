namespace QuizRuLet.Core.Abstractions;

public interface IAiApiProvider
{
    (string Result, string Error) GetContentFromJson(string json);
    Task<(string Response, string Error)> SendRequest(string prompt);
    Task<(string Result, string Error)> SendRequestAndGetResult(string userMessage);
}
