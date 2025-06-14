using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using QuizRuLet.Core.Abstractions;

namespace QuizRuLet.Infrastrucrture;


public class AiApiProvider : IAiApiProvider
{
    private readonly IConfiguration _configuration;
    private readonly AiApiOptions _aiApiOptions;
    private static readonly string API_URL = "https://api.gen-api.ru/api/v1/networks/claude-4";

    
    public AiApiProvider(IOptions<AiApiOptions> aiApiOptions)
    {
        _aiApiOptions = aiApiOptions.Value;
    }
    

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
                var apiKey = _aiApiOptions.ApiKey; 
                // POST запрос
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);
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



}
