using AutoMapper;
using Lunatic.Application.Persistence.WriteSide;
using Lunatic.Domain.DomainEvents.Task;
using MediatR;

namespace Lunatic.Application.Features.Tasks.Commands.MarkTaskAsInProgress {
	public class MarkTaskAsInProgressCommandHandler(
		ITaskRepository taskRepository,
		IPublisher publisher,
		IMapper mapper,
		IUnitOfWork unitOfWork) : IRequestHandler<MarkTaskAsInProgressCommand, MarkTaskAsInProgressCommandResponse> {

		private readonly ITaskRepository taskRepository = taskRepository;
		private readonly IPublisher publisher = publisher;
		private readonly IMapper mapper = mapper;
		private readonly IUnitOfWork unitOfWork = unitOfWork;

		public async Task<MarkTaskAsInProgressCommandResponse> Handle(MarkTaskAsInProgressCommand request, CancellationToken cancellationToken) {
			var taskResult = await taskRepository.FindByIdAsync(request.TaskId);
			if (!taskResult.IsSuccess) {
				return new MarkTaskAsInProgressCommandResponse {
					Success = false,
					Message = taskResult.Error
				};
			}

			taskResult.Value.SetStatus(Domain.Entities.TaskStatus.IN_PROGRESS);
			var dbTaskResult = await taskRepository.UpdateAsync(taskResult.Value);
			if (!dbTaskResult.IsSuccess) {
				return new MarkTaskAsInProgressCommandResponse {
					Success = false,
					Message = dbTaskResult.Error
				};
			}

			await unitOfWork.SaveChangesAsync(cancellationToken);

			await publisher.Publish(mapper.Map<TaskUpdatedDomainEvent>(dbTaskResult.Value), cancellationToken);

			return new MarkTaskAsInProgressCommandResponse { Success = true };
		}
	}
}
