using System;
using System.Reflection;
using QuizRuLet.Core.Models;
using QuizRuLet.Core.Abstractions;
using System.Security.Cryptography.X509Certificates;

namespace QuizRuLet.Application.Services;

public class CardSetCreationService : ICardSetCreationService
{
    public List<Card> Create(string inputData, string pairSeparator, string lineSeparator)
    {
        return GetCardsSet(inputData, pairSeparator, lineSeparator);
    }

    private List<Card> GetCardsSet(string inputData, string pairSeparator, string lineSeparator)
    {
        var pairs = inputData.Split(lineSeparator);     // разбили на пары
        var cards = new List<Card>();

        foreach (var pair in pairs)
        {
            if (!string.IsNullOrEmpty(pair))
            {
                var card = GetCard(pair, pairSeparator);    // TODO VALIDATION
                cards.Add(card);
            }
        }

        return cards;
    }

    private Card GetCard(string pair, string pairSeparator)
    {
        var splitPair = pair.Split(pairSeparator);      // разбили на термин и определение

        var id = Guid.NewGuid();
        var frontSide = splitPair[0];
        var backSide = splitPair[1];

        var card = Card.Create(id, frontSide, backSide, false).Card;   // TODO VALIDATION

        return card;
    }
}
