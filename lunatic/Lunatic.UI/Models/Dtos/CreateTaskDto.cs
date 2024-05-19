using Lunatic.UI.Models.Shared;

namespace Lunatic.UI.Models.Dtos {
	public class CreateTaskDto {
		public Guid UserId { get; set; } //owner id
		public Guid ProjectId { get; set; }
		public string Title { get; set; } = null!;
		public string Description { get; set; } = null!;
		public string Section { get; set; } = null!;
		public TaskPriority Priority { get; set; }
		public List<string> Tags { get; set; } = null!;
		public List<Guid> AssigneeIds { get; set; } = null!;
		public DateTime PlannedStartDate { get; set; } //for UI 
		public DateTime PlannedEndDate { get; set; }
	}
}
