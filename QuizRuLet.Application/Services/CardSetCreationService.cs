using System;
using System.Reflection;
using QuizRuLet.Core.Models;
using QuizRuLet.Core.Abstractions;
using System.Security.Cryptography.X509Certificates;

namespace QuizRuLet.Application.Services;

public class CardSetCreationService : ICardSetCreationService
{
    public (List<Card>? Cards, string Error) Create(string inputData, string pairSeparator, string lineSeparator)
    {
        (List<Card>? Cards, string Error) result = (null, "");
        try
        {
            var cards = GetCardsSet(inputData, pairSeparator, lineSeparator);
            result = (cards, "");
        }
        catch(IndexOutOfRangeException ex)
        {
            result = (null, "Ошибка при обработке текста разделителями.");
        }
        catch(Exception ex)
        {
            result = (null, ex.Message);
        }

        return result;
    }

    private List<Card> GetCardsSet(string inputData, string pairSeparator, string lineSeparator)
    {
        var pairs = inputData.Split(lineSeparator);     // разбили на пары
        var cards = new List<Card>();

        foreach (var pair in pairs)
        {
            if (!string.IsNullOrEmpty(pair))
            {
                var cardCreation = GetCard(pair, pairSeparator);
                
                if (!string.IsNullOrEmpty(cardCreation.Error)) 
                    throw new Exception(cardCreation.Error);
                    
                cards.Add(cardCreation.Card);
            }
        }

        return cards;
    }

    private (Card Card, string Error) GetCard(string pair, string pairSeparator)
    {
        var splitPair = pair.Split(pairSeparator);      // разбили на термин и определение

        var id = Guid.NewGuid();
        var frontSide = splitPair[0];
        var backSide = splitPair[1];

        var cardCreation = Card.Create(id, frontSide, backSide, false);

        return cardCreation;
    }
}
