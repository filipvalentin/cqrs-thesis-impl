namespace Lunatic.UI.Models.Dtos {
	public class CommentDto {
		public Guid CommentId { get; set; } = default!;
		public Guid TaskId { get; set; } = default!;
		public Guid AuthorId { get; set; } = default!; //author
		public string Content { get; set; } = default!;
		public List<Guid> EmoteIds { get; set; } = default!;
		public DateTime CreatedDate { get; set; } = default!;
		public DateTime LastModifiedDate { get; set; } = default!;
	}
}
