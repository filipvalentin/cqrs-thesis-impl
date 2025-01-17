﻿using AutoMapper;
using Lunatic.Application.Models.ReadModels.Tasks;
using Lunatic.Application.Persistence.ReadSide.Task;
using Lunatic.Application.Utils.Services;
using Lunatic.Domain.DomainEvents.Task;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Lunatic.Application.Features.Tasks.Events {
	internal class TaskCreatedDomainEventHandler(
		ITaskReadSideRepository taskReadRepository,
		ILogger<TaskCreatedDomainEventHandler> logger,
		IMapper mapper,
		IEventQueueService queueService,
		IFlatTaskReadSideRepository flatTaskReadRepository) : INotificationHandler<TaskCreatedDomainEvent> {

		private readonly ITaskReadSideRepository taskReadRepository = taskReadRepository;
		private readonly IFlatTaskReadSideRepository flatTaskReadRepository = flatTaskReadRepository;
		private readonly ILogger<TaskCreatedDomainEventHandler> logger = logger;
		private readonly IMapper mapper = mapper;
		private readonly IEventQueueService queueService = queueService;

		public async Task Handle(TaskCreatedDomainEvent domainEvent, CancellationToken cancellationToken) {
			var status = await taskReadRepository.AddAsync(mapper.Map<TaskReadModel>(domainEvent));
			if (!status.IsSuccess) {
				logger.LogError("Failed to add task with id {taskId}. Error: {Error}", domainEvent.Id, status.Error);
				queueService.Enqueue(domainEvent);
			}
			var flatStatus = await flatTaskReadRepository.AddAsync(mapper.Map<FlatTaskReadModel>(domainEvent));
			if (!flatStatus.IsSuccess) {
				logger.LogError("Failed to add flat task with id {taskId}. Error: {Error}", domainEvent.Id, flatStatus.Error);
				queueService.Enqueue(domainEvent);
			}
		}
	}
}
