using Lunatic.Application.Persistence.ReadSide;
using Lunatic.Application.Utils.Services;
using Lunatic.Domain.DomainEvents.Project;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Lunatic.Application.Features.Projects.Events {
	public class TaskSectionCardAddedDomainEventHandler(
		IProjectReadSideRepository projectReadRepository, 
		ILogger<TaskSectionCardAddedDomainEventHandler> logger, 
		IEventQueueService eventQueueService) : INotificationHandler<TaskSectionCardAddedDomainEvent> {
		
		private readonly IProjectReadSideRepository projectReadRepository = projectReadRepository;
		private readonly ILogger<TaskSectionCardAddedDomainEventHandler> logger = logger;
		private readonly IEventQueueService eventQueueService = eventQueueService;

		public async Task Handle(TaskSectionCardAddedDomainEvent domainEvent, CancellationToken cancellationToken) {
			var projectResult = await projectReadRepository.FindByIdAsync(domainEvent.Id);
			if (!projectResult.IsSuccess) {
				logger.LogError("Failed to find project with id {projectId}", domainEvent.Id);
				eventQueueService.Enqueue(domainEvent);
				return;
			}
			projectResult.Value.TaskSectionCards.Add(domainEvent.SectionCardName);
			var projectUpdateResult = await projectReadRepository.UpdateAsync(domainEvent.Id, projectResult.Value);
			if (!projectUpdateResult.IsSuccess) {
				logger.LogError("Failed to update project with id {projectId}", domainEvent.Id);
				eventQueueService.Enqueue(domainEvent);
				return;
			}
		}
	}
}
