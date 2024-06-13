using AutoMapper;
using Lunatic.Application.Persistence.WriteSide;
using Lunatic.Domain.DomainEvents.Project;
using Lunatic.Domain.DomainEvents.Task;
using MediatR;

namespace Lunatic.Application.Features.Projects.Commands.DeleteTask {
	public class DeleteTaskCommandHandler(
		IProjectRepository projectRepository,
		ITaskRepository taskRepository,
		IPublisher publisher,
		IMapper mapper,
		IUnitOfWork unitOfWork) : IRequestHandler<DeleteProjectTaskCommand, DeleteTaskCommandResponse> {

		private readonly IProjectRepository projectRepository = projectRepository;
		private readonly ITaskRepository taskRepository = taskRepository;
		private readonly IPublisher publisher = publisher;
		private readonly IMapper mapper = mapper;
		private readonly IUnitOfWork unitOfWork = unitOfWork;

		public async Task<DeleteTaskCommandResponse> Handle(DeleteProjectTaskCommand request, CancellationToken cancellationToken) {

			var projectResult = await projectRepository.FindByIdAsync(request.ProjectId);
			if (!projectResult.IsSuccess) {
				return new DeleteTaskCommandResponse {
					Success = false,
					Message = projectResult.Error
				};
			}
			var project = projectResult.Value;
			project.RemoveTask(request.TaskId);
			var projectUpdatedResult = await projectRepository.UpdateAsync(project);
			if (!projectUpdatedResult.IsSuccess) {
				return new DeleteTaskCommandResponse {
					Success = false,
					Message = projectUpdatedResult.Error
				};
			}

			var taskDeletedResult = await taskRepository.DeleteAsync(request.TaskId);
			if (!taskDeletedResult.IsSuccess) {
				return new DeleteTaskCommandResponse {
					Success = false,
					Message = taskDeletedResult.Error
				};
			}

			await unitOfWork.SaveChangesAsync(cancellationToken);

			await publisher.Publish(mapper.Map<ProjectUpdatedDomainEvent>(project), cancellationToken);
			await publisher.Publish(new TaskDeletedDomainEvent(request.TaskId, taskDeletedResult.Value.CommentIds), cancellationToken);

			return new DeleteTaskCommandResponse {
				Success = true
			};
		}
	}
}
