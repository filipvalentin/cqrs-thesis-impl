namespace Lunatic.Application.Models.ReadModels {
	public class CommentReadModel {
		public Guid Id { get; set; }
		public Guid TaskId { get; set; }
		public string Content { get; set; } = string.Empty;
		public Guid CreatedByUserId { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime LastModifiedDate { get; set; }
	}
}
