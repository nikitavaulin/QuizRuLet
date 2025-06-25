using System;
using System.Reflection;
using QuizRuLet.Core.Models;
using QuizRuLet.Core.Abstractions;
using System.Security.Cryptography.X509Certificates;

namespace QuizRuLet.Application.Services;

public class CardSetCreationService : ICardSetCreationService
{
    // Создание набора карточек
    public (List<Card>? Cards, string Error) Create(string inputData, string pairSeparator, string lineSeparator)
    {
        (List<Card>? Cards, string Error) result = (null, "");  // инициализация результата

        try
        {
            var cards = GetCardsSet(inputData, pairSeparator, lineSeparator);       // получение набора карточек
            result = (cards, "");
        }
        catch (IndexOutOfRangeException)
        {
            result = (null, "Ошибка при обработке текста разделителями.");
        }
        catch (Exception ex)
        {
            result = (null, ex.Message);
        }

        return result;
    }

    // Создание списка карточек
    private static List<Card> GetCardsSet(string inputData, string pairSeparator, string lineSeparator)
    {
        var pairs = inputData.Split(lineSeparator);                 // разделение текста на карточки
        var cards = new List<Card>();

        foreach (var pair in pairs) 
        {
            if (!string.IsNullOrEmpty(pair))                        // проверка, что строка не пустая
            {
                var cardCreation = GetCard(pair, pairSeparator);    // получение объекта карточки
                
                // если есть ошибка, выбрасываем исключение
                if (!string.IsNullOrEmpty(cardCreation.Error)) throw new Exception(cardCreation.Error);
                    
                cards.Add(cardCreation.Card);                       // добавление созданной карточки в список
            }
        }

        return cards;
    }

    // Создание объекта карточки
    private static (Card Card, string Error) GetCard(string pair, string pairSeparator)
    {
        // разделение пары
        var splitPair = pair.Split(pairSeparator);

        // поля карточек        
        var id = Guid.NewGuid();
        var frontSide = splitPair[0];
        var backSide = splitPair[1];

        // создание карточки
        var cardCreation = Card.Create(id, frontSide, backSide, false);

        return cardCreation;
    }
}