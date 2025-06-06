using System;
using System.ComponentModel.DataAnnotations;

namespace QuizRuLet.API.Contracts;

public record UserRegisterRequest
(
    [Required] string login,
    [Required] string password
);

