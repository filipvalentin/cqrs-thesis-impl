using AutoMapper;
using Lunatic.Application.Persistence.WriteSide;
using Lunatic.Domain.DomainEvents.Project;
using Lunatic.Domain.DomainEvents.Team;
using MediatR;

namespace Lunatic.Application.Features.Teams.Commands.DeleteTeamProject {
	public class DeleteProjectCommandHandler(ITeamRepository teamRepository, IProjectRepository projectRepository, IPublisher publisher, IMapper mapper) : IRequestHandler<DeleteProjectCommand, DeleteProjectCommandResponse> {
		
		private readonly ITeamRepository teamRepository = teamRepository;
		private readonly IProjectRepository projectRepository = projectRepository;
		private readonly IPublisher publisher = publisher;
		private readonly IMapper mapper = mapper;

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

			await publisher.Publish(mapper.Map<TeamUpdatedDomainEvent>(team), cancellationToken);
			await publisher.Publish(
				new ProjectDeletedDomainEvent(
					Id: request.ProjectId,
					TaskIds: deleteProjectResult.Value.TaskIds),
				cancellationToken);

			return new DeleteProjectCommandResponse {
				Success = true
			};
		}
	}
}
