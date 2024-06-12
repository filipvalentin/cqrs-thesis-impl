using Lunatic.Application.Persistence.WriteSide;
using Lunatic.Domain.DomainEvents.Project;
using MediatR;

namespace Lunatic.Application.Features.Teams.Commands.DeleteTeamProject {
	public class DeleteTeamProjectCommandHandler : IRequestHandler<DeleteProjectCommand, DeleteProjectCommandResponse> {
		private readonly ITeamRepository teamRepository;
		private readonly IProjectRepository projectRepository;
		private readonly IPublisher publisher;

		public DeleteTeamProjectCommandHandler(ITeamRepository teamRepository, IProjectRepository projectRepository, IPublisher publisher) {
			this.teamRepository = teamRepository;
			this.projectRepository = projectRepository;
			this.publisher = publisher;
		}

		public async Task<DeleteProjectCommandResponse> Handle(DeleteProjectCommand request, CancellationToken cancellationToken) {
			var teamResult = await teamRepository.FindByIdAsync(request.TeamId);
			if (!teamResult.IsSuccess) {
				return new DeleteProjectCommandResponse {
					Success = false,
					Message = teamResult.Error
				};
			}

			var team = teamResult.Value;
			team.RemoveProject(request.ProjectId);
			var updateTeamResult = await teamRepository.UpdateAsync(team);
			if (!updateTeamResult.IsSuccess) {
				return new DeleteProjectCommandResponse {
					Success = false,
					Message = updateTeamResult.Error
				};
			}

			var deleteProjectResult = await projectRepository.DeleteAsync(request.ProjectId);
			if (!deleteProjectResult.IsSuccess) {
				return new DeleteProjectCommandResponse {
					Success = false,
					Message = deleteProjectResult.Error
				};
			}

			await publisher.Publish(
				new ProjectDeletedDomainEvent(
					Id: request.ProjectId,
					TaskIds: deleteProjectResult.Value.TaskIds,
					Cascaded: false,
					TeamId: request.TeamId),
				cancellationToken);

			return new DeleteProjectCommandResponse {
				Success = true
			};
		}
	}
}
