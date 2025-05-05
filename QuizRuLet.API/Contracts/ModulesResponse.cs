using System;

namespace QuizRuLet.API.Contracts
{
    public record ModulesResponse 
    (
        Guid Id,    
        string Name,
        string Description,
        int Progress
    );
}
