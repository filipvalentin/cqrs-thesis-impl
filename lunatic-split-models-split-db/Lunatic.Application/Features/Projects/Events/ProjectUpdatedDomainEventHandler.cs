using AutoMapper;
using Lunatic.Application.Models.ReadModels;
using Lunatic.Application.Persistence.ReadSide;
using Lunatic.Application.Utils.Services;
using Lunatic.Domain.DomainEvents.Project;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Lunatic.Application.Features.Projects.Events {
	public class ProjectUpdatedDomainEventHandler(
		IProjectReadSideRepository projectReadRepository,
		ILogger<ProjectUpdatedDomainEventHandler> logger,
		IMapper mapper,
		IEventQueueService queueService) : INotificationHandler<ProjectUpdatedDomainEvent> {

		private readonly IProjectReadSideRepository projectReadRepository = projectReadRepository;
		private readonly ILogger<ProjectUpdatedDomainEventHandler> logger = logger;
		private readonly IMapper mapper = mapper;
		private readonly IEventQueueService queueService = queueService;

		public async Task Handle(ProjectUpdatedDomainEvent domainEvent, CancellationToken cancellationToken) {
			var updateProjectResult = await projectReadRepository.UpdateAsync(domainEvent.Id, mapper.Map<ProjectReadModel>(domainEvent));
			if(!updateProjectResult.IsSuccess) {
				logger.LogError("Failed to update project with id {projectId}", domainEvent.Id);
				queueService.Enqueue(domainEvent);
			}
		}
	}
}
