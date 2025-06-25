using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace QuizRuLet.Core.Models
{
    /// <summary>
    /// Класс доменной области: Флеш-карточка
    /// </summary>
    public class Card
    {
        /// Константы для валидации
        public const int MAX_CARD_SIDE_LENGTH = 500;            // максимальное количество символов на одной карточки
    
        public Guid Id {get;}                                   // Уникальный ID
        public string FrontSide {get;} = string.Empty;          // Лицевая сторона
        public string BackSide {get;} = string.Empty;           // Обратная сторона
        public bool IsLearned {get; set;}                       // Флаг изученности "знаю" / "не знаю"
    
    
        private Card(Guid id, string frontSide, string backSide, bool isLearned = false)
        {
            Id = id;
            FrontSide = frontSide;
            BackSide = backSide;
            IsLearned = isLearned;
        }
    
        // Создание карточки
        public static (Card Card, string Error) Create(Guid id, string frontSide, string backSide, bool isLearned = false)
        {
            // сообщение об ошибке
            var error = string.Empty; 
        
            // валидация сторон карточки
            bool condition1 = string.IsNullOrEmpty(frontSide) || frontSide.Length > MAX_CARD_SIDE_LENGTH;
            bool condition2 = string.IsNullOrEmpty(backSide) || backSide.Length > MAX_CARD_SIDE_LENGTH;
        
            if (condition1 || condition2)
            {
                error = $"Текст на карточке не может быть пустым или больше {MAX_CARD_SIDE_LENGTH} символов.";
            }
        
            // создание объекта
            var card = new Card(id, frontSide, backSide, isLearned);
        
            return (card, error);
        }
    }

}
