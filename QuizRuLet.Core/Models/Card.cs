using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace QuizRuLet.Core.Models;

public class Card
{
    public const int MAX_CARD_SIDE_LENGTH = 500;
    
    
    public Guid Id {get;}
    
    public string FrontSide {get;} = string.Empty;
    
    public string BackSide {get;} = string.Empty;
    
    public bool IsLearned {get; set;}
    
    
    public Card() {}
    
    private Card(Guid id, string frontSide, string backSide)
    {
        Id = id;
        FrontSide = frontSide;
        BackSide = backSide;
        IsLearned = false;
    }
    
    public static (Card Card, string Error) Create(Guid id, string frontSide, string backSide)
    {
        var error = string.Empty;
        
        // валидация сторон карточки
        bool condition1 = string.IsNullOrEmpty(frontSide) || frontSide.Length > MAX_CARD_SIDE_LENGTH;
        bool condition2 = string.IsNullOrEmpty(backSide) || backSide.Length > MAX_CARD_SIDE_LENGTH;
        
        if (condition1 || condition2)
        {
            error = $"Текст на карточке не может быть пустым или больше {MAX_CARD_SIDE_LENGTH} символов.";
        }
        
        var card = new Card(id, frontSide, backSide);
        
        return (card, error);
    }
}
