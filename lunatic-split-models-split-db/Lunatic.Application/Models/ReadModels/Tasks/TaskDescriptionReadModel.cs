using Lunatic.Domain.Entities;

namespace Lunatic.Application.Models.ReadModels.Tasks {
	public class TaskDescriptionReadModel {
		public Guid Id { get; set; }
		public string TaskSectionCard { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public TaskPriority Priority { get; set; }
		public List<string> Tags { get; set; }
	}
}
