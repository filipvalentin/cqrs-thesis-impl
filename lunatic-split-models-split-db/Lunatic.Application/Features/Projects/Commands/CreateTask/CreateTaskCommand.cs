using Lunatic.Domain.Entities;
using MediatR;

namespace Lunatic.Application.Features.Projects.Commands.CreateTask {
	public class CreateTaskCommand : IRequest<CreateTaskCommandResponse> {
		public Guid UserId { get; set; } = default!;
		public Guid ProjectId { get; set; } = default!;

		public string Section { get; set; } = default!;

		public string Title { get; set; } = default!;
		public string Description { get; set; } = default!;
		public TaskPriority Priority { get; set; } = default!;

		public List<string> Tags { get; set; } = default!;
		public List<Guid> AssigneeIds { get; set; } = default!;

		public DateTime PlannedStartDate { get; set; } = default!;
		public DateTime PlannedEndDate { get; set; } = default!;
	}
}
