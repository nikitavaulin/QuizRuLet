using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using QuizRuLet.Core.Abstractions;

namespace QuizRuLet.Infrastrucrture;

/// <summary>
/// Класс для взаимодействия с API большой языковой модели
/// </summary>
public class AiApiProvider : IAiApiProvider
{
    // Параметры API
    private readonly AiApiOptions _aiApiOptions;

    // URL-адрес API для отправки запросов к БЯМ
    private static readonly string API_URL = "https://api.gen-api.ru/api/v1/networks/claude-4";   

    /// Внедрение зависимостей
    public AiApiProvider(IOptions<AiApiOptions> aiApiOptions)
    {
        _aiApiOptions = aiApiOptions.Value;
    }

    /// Отправка запроса к API и получение результата в виде текста.
    public async Task<(string Result, string Error)> SendRequestAndGetResult(string userMessage)
    {
        // отправка запроса к API и получение ответа
        var response = await SendRequest(userMessage);

        var error = response.Error;                     // сообщение об ошибке
        var jsonResult = response.Response;             // JSON-ответ от API

        if (string.IsNullOrEmpty(error))                // успешно
        {
            // парсинг JSON-ответа для извлечения содержимого
            var contentResponse = GetContentFromJson(jsonResult);

            // возврат с валидацией
            return !string.IsNullOrEmpty(contentResponse.Error)
                ? ("", contentResponse.Error)
                : (contentResponse.Result, "");
        }

        return ("", error); // если ошибка
    }

    /// Отправка HTTP-запроса к API
    public async Task<(string Response, string Error)> SendRequest(string prompt)
    {
         /// Отправка HTTP-запроса к API
        using (HttpClient client = new HttpClient())       // клиент для отправки HTTP-запросов
        {
            // Формирование тело запроса
            var requestBody = new
            {
                is_sync = true,
                model = "claude-sonnet-4-20250522",
                messages = new[]
                {
                    new
                    {
                        role = "user",
                        type = "text",
                        content = prompt
                    }
                },
                max_token_to_sample = 1000,
                temperature = 0.7
            };

            // Сериализуем тело запроса в JSON
            string jsonBody = System.Text.Json.JsonSerializer.Serialize(requestBody);
            HttpContent content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            try
            {
                var apiKey = _aiApiOptions.ApiKey;       // ключ API
                
                // заголовок авторизации с использованием Bearer-токена
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);

                // Отправка POST-запрос к API
                HttpResponseMessage response = await client.PostAsync(API_URL, content);

                // Получение ответа от API
                string responseBody = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)       // успешно
                {
                    return (responseBody, "");          // возврат JSON-ответ
                }
                else                                    // ошибка
                {
                    throw new Exception($"Error: {response.StatusCode}, Details: {responseBody}");
                }
            }
            catch (Exception exception)
            {
                return ("", exception.Message);         // сообщение об ошибке
            }
        }
    }

    /// Извлечение содержимого из JSON-ответа API
    public (string Result, string Error) GetContentFromJson(string json)
    {
        var error = string.Empty;                   // сообщениие об ошибке
        var result = string.Empty;                  // результат

        try
        {
            // парсинг JSON-ответа
            using JsonDocument document = JsonDocument.Parse(json);

            // извлечение данных из JSON
            result = document.RootElement
                .GetProperty("response")[0]
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString() ?? throw new Exception("Не удалось извлечь контент"); // исключение если пустой
        }
        catch (Exception ex)
        {
            error = ex.Message;             // сообщение об ошибке
        }

        return (result, error);
    }
}