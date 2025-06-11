using System;

namespace QuizRuLet.API.Contracts;

public record CardSetSaveRequest
(
    List<CardCreateRequest> Cards
);
