using Lunatic.Application.Persistence.WriteSide;
using MediatR;

namespace Lunatic.Application.Features.Projects.Commands.DeleteTask {
	public class DeleteTaskCommandHandler : IRequestHandler<DeleteProjectTaskCommand, DeleteTaskCommandResponse> {
		private readonly IProjectRepository projectRepository;
		private readonly ITaskRepository taskRepository;
		private readonly IPublisher publisher;

		public DeleteTaskCommandHandler(IProjectRepository projectRepository, ITaskRepository taskRepository, IPublisher publisher) {
			this.projectRepository = projectRepository;
			this.taskRepository = taskRepository;
			this.publisher = publisher;
		}

		public async Task<DeleteTaskCommandResponse> Handle(DeleteProjectTaskCommand request, CancellationToken cancellationToken) {
			var validator = new DeleteTaskCommandValidator(projectRepository, taskRepository);
			var validatorResult = await validator.ValidateAsync(request, cancellationToken);

			if (!validatorResult.IsValid) {
				return new DeleteTaskCommandResponse {
					Success = false,
					ValidationErrors = validatorResult.Errors.Select(e => e.ErrorMessage).ToList()
				};
			}

			var project = (await projectRepository.FindByIdAsync(request.ProjectId)).Value;
			project.RemoveTask(request.ProjectId);
			await projectRepository.UpdateAsync(project);

			var result = await taskRepository.DeleteAsync(request.TaskId);

			if (!result.IsSuccess) {
				return new DeleteTaskCommandResponse {
					Success = false,
					ValidationErrors = new List<string> { result.Error }
				};

			}
			return new DeleteTaskCommandResponse {
				Success = true
			};
		}
	}
}
