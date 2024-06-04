
using Lunatic.Application.Features.Projects.Payload;
using Lunatic.Application.Persistence.WriteSide;
using MediatR;


namespace Lunatic.Application.Features.Tasks.Commands.UpdateTasksSection
{
    public class UpdateProjectTasksSectionCommandHandler : IRequestHandler<UpdateTaskSectionCommand, UpdateTasksSectionCommandResponse> {
		private readonly ITaskRepository taskRepository;

		private readonly IProjectRepository projectRepository;

		public UpdateProjectTasksSectionCommandHandler(ITaskRepository taskRepository, IProjectRepository projectRepository) {
			this.taskRepository = taskRepository;
			this.projectRepository = projectRepository;
		}

		public async Task<UpdateTasksSectionCommandResponse> Handle(UpdateTaskSectionCommand request, CancellationToken cancellationToken) {
			var validator = new UpdateTasksSectionCommandValidator(projectRepository);
			var validatorResult = await validator.ValidateAsync(request, cancellationToken);

			if (!validatorResult.IsValid) {
				return new UpdateTasksSectionCommandResponse {
					Success = false,
					ValidationErrors = validatorResult.Errors.Select(e => e.ErrorMessage).ToList()
				};
			}

			var projectResult = await projectRepository.FindByIdAsync(request.ProjectId);

			var tasks = (await taskRepository.GetAllAsync()).Value;
			foreach (var task in tasks) {
				if (task.TaskSectionCard == request.Section) {
					task.SetSection(request.NewSection);
					await taskRepository.UpdateAsync(task);
				}
			}

			projectResult.Value.RemoveTaskSectionCard(request.Section);
			projectResult.Value.AddTaskSectionCard(request.NewSection);

			var dbProjectResult = await projectRepository.UpdateAsync(projectResult.Value);

			return new UpdateTasksSectionCommandResponse {
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
