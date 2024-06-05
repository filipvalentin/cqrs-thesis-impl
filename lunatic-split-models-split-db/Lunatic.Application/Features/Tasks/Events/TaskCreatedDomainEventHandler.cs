using AutoMapper;
using Lunatic.Application.Models.ReadModels.Tasks;
using Lunatic.Application.Persistence.ReadSide.Task;
using Lunatic.Application.Persistence.WriteSide;
using Lunatic.Domain.DomainEvents.Task;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Lunatic.Application.Features.Tasks.Events
{
    internal class TaskCreatedDomainEventHandler : INotificationHandler<TaskCreatedDomainEvent> {
		private readonly ITaskRepository taskWriteRepository;
		private readonly ITaskReadSideRepository taskReadRepository;
		private readonly ILogger<TaskCreatedDomainEventHandler> logger;
		private readonly IMapper mapper;

		public TaskCreatedDomainEventHandler(ITaskRepository taskWriteRepository,
			ITaskReadSideRepository taskReadRepository, ILogger<TaskCreatedDomainEventHandler> logger, IMapper mapper) {
			this.taskWriteRepository = taskWriteRepository;
			this.taskReadRepository = taskReadRepository;
			this.logger = logger;
			this.mapper = mapper;
		}

		public async Task Handle(TaskCreatedDomainEvent notification, CancellationToken cancellationToken) {
			var taskResult = await taskWriteRepository.FindByIdAsync(notification.Id);

			if (!taskResult.IsSuccess) {
				logger.LogError("Task with id {Id} not found", notification.Id);
				return;
			}

			var status = await taskReadRepository.AddAsync( mapper.Map<TaskReadModel>(taskResult.Value));

			if (!status.IsSuccess) {
				logger.LogError("Failed to add task with id {Id} to read side", notification.Id);
			}
		}
	}
}
