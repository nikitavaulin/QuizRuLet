using System;

namespace QuizRuLet.API.Contracts;

public record CardLearningFlagUpdateRequest
(
    Guid CardId,
    bool IsLearned
);
