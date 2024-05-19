using Lunatic.Application.Features.Projects.Payload;
using Lunatic.Application.Persistence;
using MediatR;


namespace Lunatic.Application.Features.Projects.Commands.DeleteTaskSectionCard {
	public class DeleteTaskSectionCommandHandler : IRequestHandler<DeleteTaskSectionCommand, DeleteTaskSectionCommandResponse>
    {
        private readonly IProjectRepository projectRepository;

        private readonly ITaskRepository taskRepository;

        public DeleteTaskSectionCommandHandler(IProjectRepository projectRepository, ITaskRepository taskRepository)
        {
            this.projectRepository = projectRepository;
            this.taskRepository = taskRepository;
        }

        public async Task<DeleteTaskSectionCommandResponse> Handle(DeleteTaskSectionCommand request, CancellationToken cancellationToken)
        {
            var validator = new DeleteTaskSectionCommandValidator(projectRepository);
            var validatorResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validatorResult.IsValid)
            {
                return new DeleteTaskSectionCommandResponse
                {
                    Success = false,
                    ValidationErrors = validatorResult.Errors.Select(e => e.ErrorMessage).ToList()
                };
            }

            var project = (await projectRepository.FindByIdAsync(request.ProjectId)).Value;
            project.RemoveTaskSectionCard(request.Section);
            var dbProjectResult = await projectRepository.UpdateAsync(project);

            var tasks = (await taskRepository.GetAllAsync()).Value;
            foreach (var task in tasks)
            {
                if (task.TaskSectionCard == request.Section)
                {
                    await taskRepository.DeleteAsync(task.Id);
                }
            }

            return new DeleteTaskSectionCommandResponse
            {
                Success = true,
                Project = new ProjectDto
                {
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
