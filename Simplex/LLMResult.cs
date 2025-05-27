namespace Domain.Entities
{
	public class LLMResult
	{
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string FilePath { get; set; } = string.Empty;
        public string LLMResultText { get; set; } = string.Empty;
        public string ModelUsed { get; set; } = string.Empty;
        public string PromptUsed { get; set; } = string.Empty;
    }
}
