using System;

namespace QuizRuLet.API.Contracts;

public record CardsDataImportRequest(
    string Data,
    string LineSeparator,
    string PairSeparator
);

public record CardsDataAiImportRequest(
    string Data,
    int CountCards
);