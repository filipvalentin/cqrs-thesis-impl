using Lunatic.Application.Persistence.ReadSide;
using Lunatic.Application.Utils.Services;
using Lunatic.Domain.DomainEvents.Project;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Lunatic.Application.Features.Projects.Events {
	public class TaskSectionCardRenamedDomainEventHandler(
		IProjectReadSideRepository projectReadRepository,
		ILogger<TaskSectionCardRenamedDomainEventHandler> logger,
		IEventQueueService eventQueueService) : INotificationHandler<TaskSectionCardRenamedDomainEvent> {

		private readonly IProjectReadSideRepository projectReadRepository = projectReadRepository;
		private readonly ILogger<TaskSectionCardRenamedDomainEventHandler> logger = logger;
		private readonly IEventQueueService eventQueueService = eventQueueService;

		public async Task Handle(TaskSectionCardRenamedDomainEvent domainEvent, CancellationToken cancellationToken) {
			var projectResult = await projectReadRepository.FindByIdAsync(domainEvent.Id);
			if (!projectResult.IsSuccess) {
				logger.LogError("Failed to find project with id {projectId}", domainEvent.Id);
				eventQueueService.Enqueue(domainEvent);
				return;
			}
			projectResult.Value.TaskSectionCards.Remove(domainEvent.SectionCardName);
			projectResult.Value.TaskSectionCards.Add(domainEvent.NewSectionCardName);
			var projectUpdateResult = await projectReadRepository.UpdateAsync(domainEvent.Id, projectResult.Value);
			if (!projectUpdateResult.IsSuccess) {
				logger.LogError("Failed to update project with id {projectId}", domainEvent.Id);
				eventQueueService.Enqueue(domainEvent);
			}
			/*No need to update the tasks, a domain event is raised in the command handler*/
		}
	}
}
