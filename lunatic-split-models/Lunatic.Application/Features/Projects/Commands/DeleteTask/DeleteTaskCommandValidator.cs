
using FluentValidation;
using Lunatic.Application.Persistence;


namespace Lunatic.Application.Features.Projects.Commands.DeleteTask
{
    internal class DeleteTaskCommandValidator : AbstractValidator<DeleteProjectTaskCommand>
    {
        private readonly IProjectRepository projectRepository;

        private readonly ITaskRepository taskRepository;

        public DeleteTaskCommandValidator(IProjectRepository projectRepository, ITaskRepository taskRepository)
        {
            this.projectRepository = projectRepository;
            this.taskRepository = taskRepository;

            RuleFor(request => request.TaskId)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull().WithMessage("{PropertyName} is required.")
                .MustAsync(async (taskId, cancellationToken) => await this.taskRepository.ExistsByIdAsync(taskId))
                .WithMessage("{PropertyName} must exists.");

            RuleFor(request => request.ProjectId)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull().WithMessage("{PropertyName} is required.")
                .MustAsync(async (projectId, cancellationToken) => await this.projectRepository.ExistsByIdAsync(projectId))
                .WithMessage("{PropertyName} must exists.");

            RuleFor(request => new { request.ProjectId, request.TaskId })
                .MustAsync(async (req, cancellationToken) =>
                {
                    var project = (await this.projectRepository.FindByIdAsync(req.ProjectId)).Value;
                    return project.TaskIds.Contains(req.TaskId);
                })
                .WithMessage("Project must include TaskId.");

            ClassLevelCascadeMode = CascadeMode.Stop;
        }
    }
}
