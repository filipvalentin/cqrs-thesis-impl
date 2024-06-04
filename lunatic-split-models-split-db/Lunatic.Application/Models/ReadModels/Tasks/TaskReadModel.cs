using Lunatic.Domain.Entities;
using TaskStatus = Lunatic.Domain.Entities.TaskStatus;

namespace Lunatic.Application.Models.ReadModels.Tasks {
	public class TaskReadModel {
		public Guid Id { get; set; }
		public Guid ProjectId { get; set; }
		public Guid CreatedByUserId { get; set; }
		public string TaskSectionCard { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public TaskPriority Priority { get; set; }
		public TaskStatus Status { get; set; }
		public List<string> Tags { get; set; }
		public List<Guid> CommentIds { get; set; }
		public List<Guid> AssigneeIds { get; set; }
		public DateTime PlannedStartDate { get; set; }
		public DateTime PlannedEndDate { get; set; }
		public DateTime? StartedDate { get; set; }
		public DateTime? EndedDate { get; set; }


	}
}
