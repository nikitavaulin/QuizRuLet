using System;

namespace QuizRuLet.API.Contracts;

public record ModuleCreationRequest
(
    string Name,
    string Description,
    Guid UserId    
);
