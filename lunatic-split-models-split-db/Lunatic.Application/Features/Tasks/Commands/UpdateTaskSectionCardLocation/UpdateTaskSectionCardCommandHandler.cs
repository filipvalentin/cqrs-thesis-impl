using AutoMapper;
using Lunatic.Application.Features.Tasks.Payload;
using Lunatic.Application.Persistence.WriteSide;
using Lunatic.Domain.DomainEvents.Task;
using MediatR;

namespace Lunatic.Application.Features.Tasks.Commands.UpdateTaskSectionCardLocation {
	public class UpdateProjectTaskSectionCommandHandler(
		ITaskRepository taskRepository,
		IProjectRepository projectRepository,
		IPublisher publisher,
		IMapper mapper,
		IUnitOfWork unitOfWork) : IRequestHandler<UpdateTaskSectionCardCommand, UpdateTaskSectionCardCommandResponse> {

		private readonly ITaskRepository taskRepository = taskRepository;
		private readonly IProjectRepository projectRepository = projectRepository;
		private readonly IPublisher publisher = publisher;
		private readonly IMapper mapper = mapper;
		private readonly IUnitOfWork unitOfWork = unitOfWork;

		public async Task<UpdateTaskSectionCardCommandResponse> Handle(UpdateTaskSectionCardCommand request, CancellationToken cancellationToken) {
			var taskResult = await taskRepository.FindByIdAsync(request.TaskId);
			if (!taskResult.IsSuccess) {
				return new UpdateTaskSectionCardCommandResponse {
					Success = false,
					Message = taskResult.Error
				};
			}

			taskResult.Value.SetSection(request.Section);

			var dbTaskResult = await taskRepository.UpdateAsync(taskResult.Value);
			if (!dbTaskResult.IsSuccess) {
				return new UpdateTaskSectionCardCommandResponse {
					Success = false,
					Message = dbTaskResult.Error
				};
			}

			await unitOfWork.SaveChangesAsync(cancellationToken);

			await publisher.Publish(mapper.Map<TaskUpdatedDomainEvent>(dbTaskResult.Value), cancellationToken);

			return new UpdateTaskSectionCardCommandResponse {
				Success = true,
				Task = mapper.Map<TaskDto>(dbTaskResult.Value)
			};
		}
	}
}
