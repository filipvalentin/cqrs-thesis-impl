namespace Lunatic.Application.Models.ReadModels {
	public class CommentReadModel {
		public Guid Id { get; set; }
		public Guid TaskId { get; set; }
		public Guid CreatedByUserId { get; set; }
		public string Content { get; set; } = string.Empty;
		public DateTime CreatedDate { get; set; }
		public DateTime LastModifiedDate { get; set; }
	}
}
