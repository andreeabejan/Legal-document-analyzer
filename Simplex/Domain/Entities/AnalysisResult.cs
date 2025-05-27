namespace Domain.Entities
{

    public class AnalysisResult
    {

        public Guid Id { get; set; }
        public Guid LegalDocumentId { get; set; } 
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string FilePath { get; set; } = string.Empty;
        public string OllamaAnalysisResultText { get; set; } = string.Empty;
        public string PhiAnalysisResultText { get; set; } = string.Empty;
        public string GemmaAnalysisResultText { get; set; } = string.Empty;

    }
}
