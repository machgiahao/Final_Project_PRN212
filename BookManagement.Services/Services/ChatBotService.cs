using BookManagement.Services.IServices;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Options;

namespace BookManagement.Services.Services
{
    using System.Net.Http.Headers;
    using System.Text;
    using System.Text.Json;
    using BookManagement.Configurations;
    using Microsoft.Extensions.Options;

    public class ChatBotService : IChatBotService
    {
        private readonly HttpClient _httpClient;
        private readonly OpenAIOptions _options;


        public ChatBotService(HttpClient httpClient, IOptions<OpenAIOptions> options)
        {
            _httpClient = httpClient;
            _options = options.Value;
        }

        public async Task<string> AskAsync(string prompt)
        {
            var requestBody = new
            {
                model = _options.Model,
                messages = new[]
                {
                new { role = "user", content = prompt }
            }
            };

            var requestJson = JsonSerializer.Serialize(requestBody);
            using var request = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/chat/completions");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _options.ApiKey);
            request.Content = new StringContent(requestJson, Encoding.UTF8, "application/json");

            using var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            using var jsonDoc = JsonDocument.Parse(responseString);
            var content = jsonDoc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            return content?.Trim() ?? "";
        }
    }

}
