using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.VisualBasic;
using QuizRuLet.Core.Abstractions;

namespace QuizRuLet.Infrastrucrture;


public class AiApiProvider : IAiApiProvider
{
    private static readonly string API_KEY = "sk-DI75n0w7xYelb6g1ccGMvBkw8ctjZdIL5jHg4BI0Z7moILU4Wut4v68pLTaI";     // TODO
    private static readonly string API_URL = "https://api.gen-api.ru/api/v1/networks/claude-4";

    public async Task<(string Result, string Error)> SendRequestAndGetResult(string userMessage)
    {
        // Отправляем запрос и получаем request_id
        var response = await SendRequest(userMessage);

        var error = response.Error;
        var jsonResult = response.Response;

        if (string.IsNullOrEmpty(error))    // если ответ получен успешно
        {
            var contentResponse = GetContentFromJson(jsonResult);     // парсинг json

            return !string.IsNullOrEmpty(contentResponse.Error)
                ? ("", contentResponse.Error)
                : (contentResponse.Result, "");
        }

        return ("", error);

    }
    // public static async Task<(string Result, string Error)> SendRequestAndGetResult(string userMessage)
    // {
    //     // Отправляем запрос и получаем request_id
    //     var response = await SendRequest(userMessage);

    //     var error = response.Error;
    //     var responseResult = response.Response;

    //     if (!string.IsNullOrEmpty(error))
    //     {
    //         return ("", error);
    //     }


    //     var initialJson = System.Text.Json.JsonDocument.Parse(responseResult);
    //     string requestId = initialJson
    //         .RootElement
    //         .GetProperty("request_id")
    //         .GetInt64()
    //         .ToString();

    //     // Получаем результат
    //     var result = await GetResult(requestId);

    //     return (result, error);
    // }


    public async Task<(string Response, string Error)> SendRequest(string prompt)
    {
        using (HttpClient client = new HttpClient())
        {

            // тело
            var requestBody = new
            {
                is_sync = true,
                model = "claude-sonnet-4-20250522",
                messages = new[]
                {
                    new
                    {
                        role = "user", // Роль отправителя (пользователь)
                        type = "text",
                        content = prompt // Текст сообщения
                    }
                },
                max_token_to_sample = 1000,
                temperature = 0.7
            };

            // json сериализация
            string jsonBody = System.Text.Json.JsonSerializer.Serialize(requestBody);
            HttpContent content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            try
            {
                // POST запрос
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", API_KEY);
                HttpResponseMessage response = await client.PostAsync(API_URL, content);

                // ответ
                string responseBody = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return (responseBody, "");
                }
                else
                {
                    throw new Exception($"Error: {response.StatusCode}, Details: {responseBody}");
                }
            }
            catch (Exception exception)
            {
                return ("", exception.Message);
            }
        }
    }

    public (string Result, string Error) GetContentFromJson(string json)
    {
        var error = string.Empty;
        var result = string.Empty;
        try
        {
            using JsonDocument document = JsonDocument.Parse(json);

            result = document.RootElement
                .GetProperty("response")[0]
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString() ?? throw new Exception("Не удалось извлечь контент");

        }
        catch (Exception ex)
        {
            error = ex.Message;
        }

        return (result, error);

    }



    // public static async Task<string> CheckRequestStatus(string requestId)
    // {
    //     using (HttpClient client = new HttpClient())
    //     {
    //         // Установка авторизации через Authorization Header
    //         client.DefaultRequestHeaders.Add("X-API-Key", API_KEY);
    //         client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", API_KEY);

    //         // Формируем URL для проверки статуса
    //         string statusUrl = $"https://api.anthropic.com/v1/requests/{requestId}"; 

    //         try
    //         {
    //             // Отправляем GET-запрос
    //             HttpResponseMessage response = await client.GetAsync(statusUrl);

    //             // Читаем ответ
    //             string responseBody = await response.Content.ReadAsStringAsync();

    //             // Возвращаем результат
    //             if (response.IsSuccessStatusCode)
    //             {
    //                 return responseBody;
    //             }
    //             else
    //             {
    //                 throw new Exception($"Error: {response.StatusCode}, Details: {responseBody}");
    //             }
    //         }
    //         catch (Exception ex)
    //         {
    //             throw new Exception($"An error occurred while checking the request status: {ex.Message}");
    //         }
    //     }
    // }    


    // public static async Task<string> GetResult(string requestId)
    // {
    //     string statusResponse = await CheckRequestStatus(requestId);
    //     var statusJson = System.Text.Json.JsonDocument.Parse(statusResponse);

    //     // Проверяем статус
    //     string status = statusJson.RootElement.GetProperty("status").GetString();
    //     if (status == "completed")
    //     {
    //         // Извлекаем результат
    //         string result = statusJson.RootElement.GetProperty("completion").GetString();
    //         return result;
    //     }
    //     else if (status == "processing")
    //     {
    //         // Если запрос еще обрабатывается, ждем и повторяем проверку
    //         await Task.Delay(2000); // Ждем 2 секунды перед повторной проверкой
    //         return await GetResult(requestId);
    //     }
    //     else
    //     {
    //         throw new Exception($"Unexpected status: {status}");
    //     }
    // }    

}
