using Lunatic.UI.Models.Shared;

namespace Lunatic.UI.Models.Dtos {
	public class TaskDto {
		public Guid TaskId { get; set; }
		public Guid ProjectId { get; set; }
		public Guid CreatedByUserId { get; set; } //owner id
		public string Section { get; set; } = null!;
		public string Title { get; set; } = null!;
		public string Description { get; set; } = null!;
		public TaskPriority Priority { get; set; }
		public Shared.TaskStatus Status { get; set; }
		public List<string> Tags { get; set; } = null!;
		public List<Guid> CommentIds { get; set; } = null!;
		public List<Guid> AssigneeIds { get; set; } = null!;
		public DateTime PlannedStartDate { get; set; }
		public DateTime PlannedEndDate { get; set; }
		public DateTime? StartedDate { get; set; }
		public DateTime? EndedDate { get; set; }
	}
}