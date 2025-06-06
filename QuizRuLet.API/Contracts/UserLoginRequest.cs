using System.ComponentModel.DataAnnotations;

namespace QuizRuLet.API.Contracts;

public record UserLoginRequest
(
    [Required] string login,
    [Required] string password
);

