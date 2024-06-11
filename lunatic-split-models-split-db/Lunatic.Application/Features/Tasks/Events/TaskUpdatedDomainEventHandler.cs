using AutoMapper;
using Lunatic.Application.Models.ReadModels.Tasks;
using Lunatic.Application.Persistence.ReadSide.Task;
using Lunatic.Application.Utils.Services;
using Lunatic.Domain.DomainEvents.Task;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunatic.Application.Features.Tasks.Events {
	public class TaskUpdatedDomainEventHandler(
		ITaskReadSideRepository taskReadRepository,
		IMapper mapper,
		IEventQueueService queueService,
		ILogger<TaskUpdatedDomainEventHandler> logger) : INotificationHandler<TaskUpdatedDomainEvent> {

		private readonly ITaskReadSideRepository taskReadRepository = taskReadRepository;
		private readonly IMapper mapper = mapper;
		private readonly ILogger<TaskUpdatedDomainEventHandler> logger = logger;
		private readonly IEventQueueService queueService = queueService;

		public async Task Handle(TaskUpdatedDomainEvent notification, CancellationToken cancellationToken) {
			var updateResult = await taskReadRepository.UpdateAsync(notification.Id, mapper.Map<TaskReadModel>(notification));
			if (!updateResult.IsSuccess) {
				logger.LogError("Error from TaskReadSideRepository: {Error} when updating entity with {Id}", updateResult.Error, notification.Id);
				queueService.Enqueue(notification);
			}
		}
	}
}
