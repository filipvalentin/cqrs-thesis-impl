using MediatR;

namespace Lunatic.Application.Features.Teams.Commands.CreateProject {
	public class CreateProjectCommand : IRequest<CreateProjectCommandResponse> {
		public Guid UserId { get; set; } = default!;
		public Guid TeamId { get; set; } = default!;
		public string Title { get; set; } = default!;
		public string Description { get; set; } = default!;
	}
}
