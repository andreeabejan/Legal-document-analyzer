using Domain.Interfaces;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Infrastructure.Repositories
{
    public class OllamaModel : IOllamaModel
    {
        private readonly HttpClient _httpClient;

        public OllamaModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.Timeout = TimeSpan.FromMinutes(3); 
        }

        public async Task<string> AnalyzeText(string content, string prompt, string model = "tinyllama")
        {
            var query = $"{prompt}\n\nDocument Content:\n{content}";

            var requestBody = new
            {
                model,         // "phi", "gemini", "tinylama"
                prompt = query,
                stream = false
            };

            var response = await _httpClient.PostAsync(
                "http://localhost:11434/api/generate",
                new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json")
            );

            //response.EnsureSuccessStatusCode();

            if (!response.IsSuccessStatusCode)
            {
                var errorText = await response.Content.ReadAsStringAsync();
                throw new Exception($"Status code: {response.StatusCode}, error: {errorText}");
            }


            var result = await response.Content.ReadAsStringAsync();
            var json = JsonDocument.Parse(result);
            
            if (json.RootElement.TryGetProperty("response", out var responseProperty) && responseProperty.ValueKind == JsonValueKind.String)
            {
                return responseProperty.GetString()!;
            }

            throw new InvalidOperationException("The response property is missing or not a string.");
        }
    }
}