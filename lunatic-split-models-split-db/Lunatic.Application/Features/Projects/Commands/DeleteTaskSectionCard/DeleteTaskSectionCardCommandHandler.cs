using AutoMapper;
using Lunatic.Application.Features.Projects.Payload;
using Lunatic.Application.Persistence.WriteSide;
using Lunatic.Domain.DomainEvents.Project;
using Lunatic.Domain.DomainEvents.Task;
using MediatR;

namespace Lunatic.Application.Features.Projects.Commands.DeleteTaskSectionCard {
	public class DeleteTaskSectionCardCommandHandler(
		IProjectRepository projectRepository,
		ITaskRepository taskRepository,
		IPublisher publisher,
		IMapper mapper) : IRequestHandler<DeleteTaskSectionCardCommand, DeleteTaskSectionCardCommandResponse> {

		private readonly IProjectRepository projectRepository = projectRepository;
		private readonly ITaskRepository taskRepository = taskRepository;
		private readonly IPublisher publisher = publisher;
		private readonly IMapper mapper = mapper;

		public async Task<DeleteTaskSectionCardCommandResponse> Handle(DeleteTaskSectionCardCommand request, CancellationToken cancellationToken) {

			var projectResponse = await projectRepository.FindByIdAsync(request.ProjectId);
			if (!projectResponse.IsSuccess) {
				return new DeleteTaskSectionCardCommandResponse {
					Success = false,
					Message = projectResponse.Error
				};
			}
			var project = projectResponse.Value;
			project.RemoveTaskSectionCard(request.Section);
			var dbProjectResult = await projectRepository.UpdateAsync(project);
			if (!dbProjectResult.IsSuccess) {
				return new DeleteTaskSectionCardCommandResponse {
					Success = false,
					Message = dbProjectResult.Error
				};
			}

			var tasksResult = await taskRepository.GetAllTasksByProjectIdAndSectionCardAsync(request.ProjectId, request.Section);
			if (!tasksResult.IsSuccess) {
				return new DeleteTaskSectionCardCommandResponse {
					Success = false,
					Message = tasksResult.Error
				};
			}
			var tasks = tasksResult.Value;
			foreach (var task in tasks) {
				var taskDeletedResult = await taskRepository.DeleteAsync(task.Id);
				if (!taskDeletedResult.IsSuccess) {
					return new DeleteTaskSectionCardCommandResponse {
						Success = false,
						Message = taskDeletedResult.Error
					};
				}
				await publisher.Publish(
					new TaskDeletedDomainEvent(Id: task.Id,
											CommentIds: task.CommentIds,
											Cascaded: false,
											ProjectId: request.ProjectId),
					cancellationToken);

			}

			await publisher.Publish(
				new TaskSectionCardRemovedDomainEvent(Id: request.ProjectId, SectionCardName: request.Section),
				cancellationToken);

			return new DeleteTaskSectionCardCommandResponse {
				Success = true,
				Project = mapper.Map<ProjectDto>(dbProjectResult.Value)
			};
		}
	}
}
