
using Domain.Interfaces;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public class GeminiApiClient : ITextAnalyzer
{
    private readonly string _apiKey;
    private readonly HttpClient _httpClient;

    public GeminiApiClient(string apiKey)
    {
        _apiKey = apiKey;
        _httpClient = new HttpClient();
    }

    public string SourceName => "Gemini";

    public async Task<string> AnalyzeTextAsync(string content, string prompt)
    {
        var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key={_apiKey}";

        var requestObject = new
        {
            contents = new[]
            {
                new {
                    parts = new[]
                    {
                        new { text = $"{prompt}\n\n{content}" }
                    }
                }
            }
        };

        var requestBody = JsonSerializer.Serialize(requestObject);

        using var client = new HttpClient();
        var contentPayload = new StringContent(requestBody, Encoding.UTF8, "application/json");

        var response = await client.PostAsync(url, contentPayload);
        var responseBody = await response.Content.ReadAsStringAsync();

        using var doc = JsonDocument.Parse(responseBody);

        var text = doc.RootElement
                      .GetProperty("candidates")[0]
                      .GetProperty("content")
                      .GetProperty("parts")[0]
                      .GetProperty("text")
                      .GetString();

        return text ?? "No response text found.";

    }
}
