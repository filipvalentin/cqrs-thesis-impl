using AutoMapper;
using Lunatic.Application.Models.ReadModels;
using Lunatic.Application.Persistence.ReadSide;
using Lunatic.Application.Utils.Services;
using Lunatic.Domain.DomainEvents.Project;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Lunatic.Application.Features.Projects.Events {
	public class ProjectCreatedDomainEventHandler(
		IProjectReadSideRepository projectReadRepository,
		ILogger<ProjectCreatedDomainEventHandler> logger,
		IMapper mapper,
		IEventQueueService queueService) : INotificationHandler<ProjectCreatedDomainEvent> {

		private readonly IProjectReadSideRepository projectReadRepository = projectReadRepository;
		private readonly ILogger<ProjectCreatedDomainEventHandler> logger = logger;
		private readonly IMapper mapper = mapper;
		private readonly IEventQueueService queueService = queueService;

		public async Task Handle(ProjectCreatedDomainEvent domainEvent, CancellationToken cancellationToken) {
			var projectResult = await projectReadRepository.AddAsync(mapper.Map<ProjectReadModel>(domainEvent));
			if (!projectResult.IsSuccess) {
				logger.LogError("Error from ProjectReadSideRepository: {Error} when adding entity with {Id}", projectResult.Error, domainEvent.Id);
				queueService.Enqueue(domainEvent);
			}
		}
	}
}
