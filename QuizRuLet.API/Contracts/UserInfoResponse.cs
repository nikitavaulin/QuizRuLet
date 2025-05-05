using System;

namespace QuizRuLet.API.Contracts;

public record UserInfoResponse
(
    Guid Id,
    string Login
);
