using AutoMapper;
using Lunatic.Application.Features.Projects.Payload;
using Lunatic.Application.Persistence.WriteSide;
using MediatR;

namespace Lunatic.Application.Features.Projects.Commands.RenameTaskSection {
	public class RenameTaskSectionCardCommandHandler : IRequestHandler<RenameTaskSectionCardCommand, RenameTaskSectionCardCommandResponse> {
		private readonly ITaskRepository taskRepository;
		private readonly IProjectRepository projectRepository;
		private readonly IPublisher publisher;
		private readonly IMapper mapper;

		public RenameTaskSectionCardCommandHandler(ITaskRepository taskRepository, IProjectRepository projectRepository, 
			IPublisher publisher, IMapper mapper) {
			this.taskRepository = taskRepository;
			this.projectRepository = projectRepository;
			this.publisher = publisher;
			this.mapper = mapper;
		}

		public async Task<RenameTaskSectionCardCommandResponse> Handle(RenameTaskSectionCardCommand request, CancellationToken cancellationToken) {
			var validator = new RenameTaskSectionCardCommandValidator(projectRepository);
			var validatorResult = await validator.ValidateAsync(request, cancellationToken);

			if (!validatorResult.IsValid) {
				return new RenameTaskSectionCardCommandResponse {
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

			return new RenameTaskSectionCardCommandResponse {
				Success = true,
				Project = mapper.Map<ProjectDto>(dbProjectResult.Value)
			};
		}
	}
}
