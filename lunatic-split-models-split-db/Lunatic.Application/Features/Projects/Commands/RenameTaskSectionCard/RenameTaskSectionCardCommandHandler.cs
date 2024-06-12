using AutoMapper;
using Lunatic.Application.Features.Projects.Payload;
using Lunatic.Application.Persistence.WriteSide;
using Lunatic.Domain.DomainEvents.Project;
using Lunatic.Domain.DomainEvents.Task;
using MediatR;

namespace Lunatic.Application.Features.Projects.Commands.RenameTaskSection {
	public class RenameTaskSectionCardCommandHandler(
		ITaskRepository taskRepository, 
		IProjectRepository projectRepository,
		IPublisher publisher, 
		IMapper mapper) : IRequestHandler<RenameTaskSectionCardCommand, RenameTaskSectionCardCommandResponse> {

		private readonly ITaskRepository taskRepository = taskRepository;
		private readonly IProjectRepository projectRepository = projectRepository;
		private readonly IPublisher publisher = publisher;
		private readonly IMapper mapper = mapper;

		public async Task<RenameTaskSectionCardCommandResponse> Handle(RenameTaskSectionCardCommand request, CancellationToken cancellationToken) {

			var projectResult = await projectRepository.FindByIdAsync(request.ProjectId);
			if (!projectResult.IsSuccess) {
				return new RenameTaskSectionCardCommandResponse {
					Success = false,
					Message = projectResult.Error
				};
			}
			var project = projectResult.Value;
			project.RemoveTaskSectionCard(request.Section);
			project.AddTaskSectionCard(request.NewSection);
			var dbProjectResult = await projectRepository.UpdateAsync(project);
			if (!dbProjectResult.IsSuccess) {
				return new RenameTaskSectionCardCommandResponse {
					Success = false,
					Message = dbProjectResult.Error
				};
			}

			var tasksResult = await taskRepository.GetAllTasksByProjectIdAndSectionCardAsync(request.ProjectId, request.Section);
			if (!tasksResult.IsSuccess) {
				return new RenameTaskSectionCardCommandResponse {
					Success = false,
					Message = tasksResult.Error
				};
			}
			foreach (var task in tasksResult.Value) {
				task.SetSection(request.NewSection);
				var taskUpdateResult = await taskRepository.UpdateAsync(task);
				if (!taskUpdateResult.IsSuccess) {
					return new RenameTaskSectionCardCommandResponse {
						Success = false,
						Message = taskUpdateResult.Error
					};
				}
				await publisher.Publish(mapper.Map<TaskUpdatedDomainEvent>(task), cancellationToken);
			}

			await publisher.Publish(mapper.Map<ProjectUpdatedDomainEvent>(project), cancellationToken);

			return new RenameTaskSectionCardCommandResponse {
				Success = true,
				Project = mapper.Map<ProjectDto>(project)
			};
		}
	}
}
