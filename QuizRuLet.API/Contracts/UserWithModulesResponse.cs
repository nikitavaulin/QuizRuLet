namespace QuizRuLet.API.Contracts;

public record UserWithModulesResponse
(
    Guid Id, // mb remove
    string Login,
    List<ModulesResponse> Modules
);
