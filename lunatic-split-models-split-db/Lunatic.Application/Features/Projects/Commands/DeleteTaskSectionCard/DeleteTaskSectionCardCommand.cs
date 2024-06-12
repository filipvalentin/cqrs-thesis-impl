using MediatR;


namespace Lunatic.Application.Features.Projects.Commands.DeleteTaskSectionCard {
	public class DeleteTaskSectionCardCommand : IRequest<DeleteTaskSectionCardCommandResponse> {
		public Guid ProjectId { get; set; } = default!;

		public string Section { get; set; } = default!;
	}
}
