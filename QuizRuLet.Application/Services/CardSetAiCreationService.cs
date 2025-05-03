using System;
using QuizRuLet.Core.Abstractions;
using QuizRuLet.Core.Models;

namespace QuizRuLet.Application.Services;

public class CardSetAiCreationService
{
    private readonly ICardSetCreationService _creationService;


    public CardSetAiCreationService(ICardSetCreationService cardSetCreationService)
    {
        _creationService = cardSetCreationService;
    }

    public List<Card> Create(string prompt, int countCards)
    {
        string inputData = GetData(prompt, countCards);
        string sep1 = string.Empty;
        string sep2 = string.Empty;
        
        var cards = _creationService.Create(inputData, sep1, sep2);
        
        return cards;
    }
    
    public string GetData(string prompt, int countCards)
    {
        string inputData = string.Empty;
        return inputData;
    }
}
