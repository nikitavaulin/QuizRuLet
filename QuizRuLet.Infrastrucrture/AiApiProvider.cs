using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using QuizRuLet.Core.Abstractions;

namespace QuizRuLet.Infrastrucrture;

public class AiApiProvider : IAiApiProvider
{
    private static readonly string API_KEY = "sk-DI75n0w7xYelb6g1ccGMvBkw8ctjZdIL5jHg4BI0Z7moILU4Wut4v68pLTaI";     // TODO
    private static readonly string API_URL = "https://api.gen-api.ru/api/v1/networks/claude-4";


    public static async Task<(string Response, string Error)> SendRequest(string prompt)
    {
        using (HttpClient client = new HttpClient())
        {
            // загоовки
            // client.DefaultRequestHeaders.Add("Authorization", $"Bearer {API_KEY}");
            // client.DefaultRequestHeaders.Add("Content-Type", "application/json");
            // client.DefaultRequestHeaders.Add("X-API-Key", API_KEY);

            // тело
            var requestBody = new
            {
                model = "claude-sonnet-4-20250522",
                messages = new[]
                {
                    new
                    {
                        role = "user", // Роль отправителя (пользователь)
                        content = prompt // Текст сообщения
                    }
                },
                max_token_to_sample = 100,
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

}
