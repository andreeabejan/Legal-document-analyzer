namespace Domain.Interfaces
{
    public interface IOllamaModel
    {
        Task<string> AnalyzeText(string content, string promt, string model);
    }
}
