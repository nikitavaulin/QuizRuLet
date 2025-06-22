using System;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using QuizRuLet.Core.Abstractions;
using QuizRuLet.Core.Models;
using QuizRuLet.Infrastrucrture;

namespace QuizRuLet.Application.Services;

public class CardSetAiCreationService : ICardSetAiCreationService
{
    // константы для форматирования данных
    private static string START_SEPARATOR = "*START*";          // начальный разделитель 
    private static string END_SEPARATOR = "*END*";              // конечный разделитель 
    private static string LINE_SEPARATOR = "*LINE*";            // разделитель строк в наборе карточек
    private static string PAIR_SEPARATOR = "*PAIR*";            // разделитель термин-определение в карточке    
    private static string CARD_FORMAT = $"Передняя сторона карточки{PAIR_SEPARATOR}Обратная сторона карточки{LINE_SEPARATOR}"; 
    private static string CARD_SET_FORMAT = $"{START_SEPARATOR}{CARD_FORMAT}{CARD_FORMAT}и так далее{END_SEPARATOR}";

    // Внедрение зависимостей
    private readonly ICardSetCreationService _creationService;  // сервис для создания карточек из текста
    private readonly IAiApiProvider _aiProvider;                // провайдер API БЯМ

    // Внедрение зависимостей
    public CardSetAiCreationService(
        ICardSetCreationService cardSetCreationService,
        IAiApiProvider aiProvider)
    {
        _creationService = cardSetCreationService; 
        _aiProvider = aiProvider; 
    }

    /// Создание набора карточек с использованием ИИ
    public async Task<(List<Card>? Cards, string Error)> Create(string data, int countCards)
    {
        // генерация промта для отправки в ИИ
        string prompt = GetPrompt(data, countCards);

        
        var response = await GetAiResponse(prompt);                 // получение ответа от ИИ

        if (string.IsNullOrEmpty(response.Error))                   // ответ успешный
        {
            var cardSetString = GetCardSetString(response.Result);  // извлечение набора карточек из ответа ИИ

            // создание карточек
            var cardCreation = _creationService.Create(cardSetString, PAIR_SEPARATOR, LINE_SEPARATOR);

            return cardCreation;
        }

        return (null, response.Error);
    }

    /// Отправка запроса к ИИ
    private async Task<(string Result, string Error)> GetAiResponse(string prompt)
    {
        // отправка запроса к ИИ
        var response = await _aiProvider.SendRequestAndGetResult(prompt);
        
        return string.IsNullOrEmpty(response.Error)
            ? (response.Result, "")
            : ("", response.Error);
    }

    /// Извлечение набора карточек из ответа ИИ
    private static string GetCardSetString(string data)
    {
        try
        {
            // извлечение данных между разделителями
            var result = data
                .Split(START_SEPARATOR)[1]
                .Split(END_SEPARATOR)[0];

            return result; // возвращаем извлеченный текст
        }
        catch (Exception)
        {
            return string.Empty; // если ошибка => пустая строка
        }
    }

    /// Формирование промта
    private static string GetPrompt(string data, int countCards)
    {
        string intro = $"Я студент. Мне нужна твоя помощь. Мне нужно на основе конспекта, создать {countCards} штук флеш-карточек для запоминания. ";
        string goal1 = $"Пусть на одной стороне будет термин / событие / личность, а на другой строне будет определение / дата / краткое описание соответственно. ";
        string goal2 = $"Старайся, чтобы на сторонах карточек было максимум одно предложение. ";
        string format = $"Результат представь в следующем формате: \n{CARD_SET_FORMAT}\n";
        string inputData = $"Вот конспект для создания набора карточек.\n{data}";
        
        string prompt = intro + goal1 + goal2 + format + inputData;
        return prompt;
    }
}