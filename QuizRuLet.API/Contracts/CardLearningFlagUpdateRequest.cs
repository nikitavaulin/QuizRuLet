using System;

namespace QuizRuLet.API.Contracts;

public record CardLearningFlagUpdateRequest
(
    bool IsLearned
);
