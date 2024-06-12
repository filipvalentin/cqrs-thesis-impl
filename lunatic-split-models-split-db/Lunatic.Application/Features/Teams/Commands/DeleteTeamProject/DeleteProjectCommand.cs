using MediatR;

namespace Lunatic.Application.Features.Teams.Commands.DeleteTeamProject {
	public class DeleteProjectCommand : IRequest<DeleteProjectCommandResponse> {
		public Guid TeamId { get; set; }
		public Guid ProjectId { get; set; }
	}
}
