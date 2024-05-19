using Lunatic.UI.Models.Shared;


namespace Lunatic.UI.Models.ViewModels {
	public class EditTaskViewModel {
		public Guid TaskId { get; set; }
		public string Title { get; set; } = null!;
		public string Description { get; set; } = null!;
		public TaskPriority Priority { get; set; }
		public List<string> Tags { get; set; } = null!;
		public List<Guid> AssigneeIds { get; set; } = null!;
		public DateTime PlannedStartDate { get; set; } //for UI 
		public DateTime PlannedEndDate { get; set; }
	}
}
