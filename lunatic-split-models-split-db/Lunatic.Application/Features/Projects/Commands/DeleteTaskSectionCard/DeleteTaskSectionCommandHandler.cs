using AutoMapper;
using Lunatic.Application.Features.Projects.Payload;
using Lunatic.Application.Persistence.WriteSide;
using MediatR;

namespace Lunatic.Application.Features.Projects.Commands.DeleteTaskSectionCard {
	public class DeleteTaskSectionCommandHandler : IRequestHandler<DeleteTaskSectionCommand, DeleteTaskSectionCommandResponse> {
		private readonly IProjectRepository projectRepository;
		private readonly ITaskRepository taskRepository;
		private readonly IPublisher publisher;
		private readonly IMapper mapper;

		public DeleteTaskSectionCommandHandler(IProjectRepository projectRepository, ITaskRepository taskRepository, 
			IPublisher publisher, IMapper mapper) {
			this.projectRepository = projectRepository;
			this.taskRepository = taskRepository;
			this.publisher = publisher;
			this.mapper = mapper;
		}

		public async Task<DeleteTaskSectionCommandResponse> Handle(DeleteTaskSectionCommand request, CancellationToken cancellationToken) {
			//var validator = new DeleteTaskSectionCommandValidator(projectRepository);
			//var validatorResult = await validator.ValidateAsync(request, cancellationToken);

			//if (!validatorResult.IsValid) {
			//	return new DeleteTaskSectionCommandResponse {
			//		Success = false,
			//		ValidationErrors = validatorResult.Errors.Select(e => e.ErrorMessage).ToList()
			//	};
			//}

			var project = (await projectRepository.FindByIdAsync(request.ProjectId)).Value;
			project.RemoveTaskSectionCard(request.Section);
			var dbProjectResult = await projectRepository.UpdateAsync(project);

			var tasks = (await taskRepository.GetAllAsync()).Value;
			foreach (var task in tasks) {
				if (task.TaskSectionCard == request.Section) {
					await taskRepository.DeleteAsync(task.Id);
				}
			}

			return new DeleteTaskSectionCommandResponse {
				Success = true,
				Project = mapper.Map<ProjectDto>(dbProjectResult.Value)
			};
		}
	}
}
