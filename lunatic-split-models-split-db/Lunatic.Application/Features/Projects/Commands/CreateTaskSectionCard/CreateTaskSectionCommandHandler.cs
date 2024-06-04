using Lunatic.Application.Features.Projects.Payload;
using Lunatic.Application.Persistence.WriteSide;
using MediatR;


namespace Lunatic.Application.Features.Projects.Commands.CreateTaskSectionCard
{
    public class CreateTaskSectionCommandHandler : IRequestHandler<CreateTaskSectionCommand, CreateTaskSectionCommandResponse> {
		private readonly IProjectRepository projectRepository;

		public CreateTaskSectionCommandHandler(IProjectRepository projectRepository) {
			this.projectRepository = projectRepository;
		}

		public async Task<CreateTaskSectionCommandResponse> Handle(CreateTaskSectionCommand request, CancellationToken cancellationToken) {
			var validator = new CreateTaskSectionCommandValidator(projectRepository);
			var validatorResult = await validator.ValidateAsync(request, cancellationToken);

			if (!validatorResult.IsValid) {
				return new CreateTaskSectionCommandResponse {
					Success = false,
					ValidationErrors = validatorResult.Errors.Select(e => e.ErrorMessage).ToList()
				};
			}

			var project = (await projectRepository.FindByIdAsync(request.ProjectId)).Value;
			project.AddTaskSectionCard(request.Section);
			var dbProjectResult = await projectRepository.UpdateAsync(project);

			return new CreateTaskSectionCommandResponse {
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
