namespace Lunatic.Application.Models.ReadModels {
	public class CommentReadModel {
		public Guid CommentId { get; set; }
		public Guid TaskId { get; set; }
		public string Content { get; set; }
	}
}
