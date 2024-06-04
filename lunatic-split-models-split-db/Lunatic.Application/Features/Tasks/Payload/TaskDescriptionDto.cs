
using Lunatic.Domain.Entities;


namespace Lunatic.Application.Features.Tasks.Payload {
	public class TaskDescriptionDto {
		public Guid TaskId { get; set; } = default!;
		public string Section { get; set; } = default!;
		public string Title { get; set; } = default!;
		public string Description { get; set; } = default!;
		public TaskPriority Priority { get; set; } = default!;
		public List<string> Tags { get; set; } = default!;
	}
}
