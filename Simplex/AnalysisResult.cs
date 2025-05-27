namespace Domain.Entities
{

    public class AnalysisResult
    {

        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string FilePath { get; set; } = string.Empty;
        // summary, clauses, entities
        public string AnalysisResult { get; set; } = string.Empty; 

    }
}
