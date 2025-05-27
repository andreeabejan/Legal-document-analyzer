namespace Domain.Entities
{
	public class LegalDocument
	{
		public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
		public string FilePath { get; set; } = string.Empty;
	}
}
