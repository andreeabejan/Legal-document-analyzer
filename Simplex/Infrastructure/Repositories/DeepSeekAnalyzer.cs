using Domain.Interfaces;
using RestSharp;
using System;
using System.Text.Json;
using System.Threading.Tasks;

public class DeepSeekAnalyzer : ITextAnalyzer
{
    private readonly string _apiKey;
    private readonly RestClient _client;
    private readonly string _model;

    public DeepSeekAnalyzer(string apiKey, string model = "mistralai/Mistral-7B-Instruct-v0.2")
    {
        _apiKey = apiKey;
        _model = model;
        _client = new RestClient("https://api.aimlapi.com/v1");
    }

    public string SourceName => $"DeepSeek:{_model}";

    public async Task<string> AnalyzeTextAsync(string content, string prompt)
    {
        var request = new RestRequest("/chat/completions", Method.Post);
        request.AddHeader("Authorization", $"Bearer {_apiKey}");
        request.AddHeader("Content-Type", "application/json");

        var body = new
        {
            model = _model,
            messages = new[]
            {
                new { role = "user", content = $"{prompt}\n\n{content}" }
            },
            temperature = 0.7,
            max_tokens = 256
        };

        request.AddJsonBody(body);

        var response = await _client.ExecutePostAsync(request);
        if (!response.IsSuccessful)
            throw new Exception($"DeepSeek API Error: {response.StatusCode} - {response.Content}");

        var doc = JsonDocument.Parse(response.Content!);
        var root = doc.RootElement;

        var message = root
            .GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString();

        return message!;
    }
}
