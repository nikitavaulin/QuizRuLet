using System;
using System.Net.Mime;
using System.Threading.Tasks;
using QuizRuLet.Core.Abstractions;
using QuizRuLet.Core.Models;
using QuizRuLet.Infrastrucrture;

namespace QuizRuLet.Application.Services;

public class CardSetAiCreationService : ICardSetAiCreationService
// TODO
{
    private static string START_SEPARATOR = "*START*";
    private static string END_SEPARATOR = "*END*";
    private static string LINE_SEPARATOR = "*LINE*";
    private static string PAIR_SEPARATOR = "*PAIR*";
    private static string CARD_FORMAT = $"Передняя сторона карточки{PAIR_SEPARATOR}Обратная сторона карточки{LINE_SEPARATOR}";
    private static string CARD_SET_FORMAT = $"{START_SEPARATOR}{CARD_FORMAT}{CARD_FORMAT}и так далее{END_SEPARATOR}";


    private readonly ICardSetCreationService _creationService;
    private readonly IAiApiProvider _aiProvider;

    public CardSetAiCreationService(
        ICardSetCreationService cardSetCreationService,
        IAiApiProvider aiProvider)
    {
        _creationService = cardSetCreationService;
        _aiProvider = aiProvider;
    }

    // //
    // public CardSetAiCreationService(
    //     CardSetCreationService cardSetCreationService,
    //     AiApiProvider aiProvider)
    // {
    //     _creationService = cardSetCreationService;
    //     _aiProvider = aiProvider;
    // }

    public async Task<(List<Card>? Cards, string Error)> Create(string data, int countCards)
    {
        string prompt = GetPrompt(data, countCards);

        var response = await GetAiResponse(prompt);

        if (string.IsNullOrEmpty(response.Error))
        {
            var cardSetString = GetCardSetString(response.Result);
            var cards = _creationService.Create(cardSetString, PAIR_SEPARATOR, LINE_SEPARATOR);

            return (cards, "");
        }

        return (null, response.Error);
    }

    private async Task<(string Result, string Error)> GetAiResponse(string prompt)
    {
        var response = await _aiProvider.SendRequestAndGetResult(prompt);

        return !string.IsNullOrEmpty(response.Error)
            ? (response.Result, "")
            : ("", response.Error);
    }

    /// <summary>
    /// Вырезает набор карточек из результата ИИ
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    private string GetCardSetString(string data)
    {
        try
        {
            var result = data
                .Split(START_SEPARATOR)[1]
                .Split(END_SEPARATOR)[0];

            return result;
        }
        catch (Exception)
        {
            return string.Empty;
        }
    }



    /// <summary>
    /// Получение промта
    /// </summary>
    /// <param name="data"></param>
    /// <param name="countCards"></param>
    /// <returns></returns>
    private static string GetPrompt(string data, int countCards)
    {
        string intro = $"Я студент. Мне нужна твоя помощь. Мне нужно на основе коспекта, создать {countCards} штук флеш-карточек для запоминания. ";
        string goal1 = $"Пусть на одной стороне будет термин / дата / личность, а на другой строне будет определение / событие / краткое описание соответственно. ";
        string goal2 = $"Старайся, чтобы на сторонах карточек было не более 3 предложений. ";
        string format = $"Результат представь в следующем формате: \n{CARD_SET_FORMAT}\n";
        string inputData = $"Вот конспект для создания набора карточек.\n{data}";

        string prompt = intro + goal1 + goal2 + format + inputData;
        return prompt;
    }
}
