
using Lunatic.Application.Features.Projects.Payload;
using Lunatic.Application.Persistence;
using MediatR;


namespace Lunatic.Application.Features.Projects.Commands.UpdateProject {
	public class UpdateTeamProjectCommandHandler : IRequestHandler<UpdateTeamProjectCommand, UpdateTeamProjectCommandResponse> {
		private readonly IProjectRepository projectRepository;

		public UpdateTeamProjectCommandHandler(IProjectRepository projectRepository) {
			this.projectRepository = projectRepository;
		}

		public async Task<UpdateTeamProjectCommandResponse> Handle(UpdateTeamProjectCommand request, CancellationToken cancellationToken) {
			var validator = new UpdateTeamProjectCommandValidator(projectRepository);
			var validatorResult = await validator.ValidateAsync(request, cancellationToken);

			if (!validatorResult.IsValid) {
				return new UpdateTeamProjectCommandResponse {
					Success = false,
					ValidationErrors = validatorResult.Errors.Select(e => e.ErrorMessage).ToList()
				};
			}

			var projectResult = await projectRepository.FindByIdAsync(request.ProjectId);

			projectResult.Value.Update(request.Title, request.Description);

			var dbProjectResult = await projectRepository.UpdateAsync(projectResult.Value);

			return new UpdateTeamProjectCommandResponse {
				Success = true,
				Project = new ProjectDto {
					CreatedByUserId = dbProjectResult.Value.CreatedByUserId,
					ProjectId = dbProjectResult.Value.Id,
					TeamId = dbProjectResult.Value.TeamId,

					Title = dbProjectResult.Value.Title,
					Description = dbProjectResult.Value.Description,

					TaskSections = dbProjectResult.Value.TaskSectionCards,
					TaskIds = dbProjectResult.Value.TaskIds,
				}
			};
		}
	}
}
