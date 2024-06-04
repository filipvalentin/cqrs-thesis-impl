using MediatR;


namespace Lunatic.Application.Features.Projects.Commands.DeleteTask {
	public class DeleteProjectTaskCommand : IRequest<DeleteTaskCommandResponse> {
		public Guid ProjectId { get; set; }
		public Guid TaskId { get; set; }
	}
}
