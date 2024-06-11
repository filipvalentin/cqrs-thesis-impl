using AutoMapper;
using Lunatic.Application.Models.ReadModels;
using Lunatic.Application.Persistence.ReadSide;
using Lunatic.Application.Utils;
using Lunatic.Application.Utils.Services;
using Lunatic.Domain.DomainEvents.Project;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Lunatic.Application.Features.Projects.Events {
	public class ProjectCreatedDomainEventHandler(
		IProjectReadSideRepository projectReadRepository,
		ILogger<ProjectCreatedDomainEventHandler> logger,
		IMapper mapper,
		ITeamReadSideRepository teamReadRepository,
		IEventQueueService queueService) : INotificationHandler<ProjectCreatedDomainEvent> {

		private readonly IProjectReadSideRepository projectReadRepository = projectReadRepository;
		private readonly ITeamReadSideRepository teamReadRepository = teamReadRepository;
		private readonly ILogger<ProjectCreatedDomainEventHandler> logger = logger;
		private readonly IMapper mapper = mapper;
		private readonly IEventQueueService queueService = queueService;

		public async Task Handle(ProjectCreatedDomainEvent domainEvent, CancellationToken cancellationToken) {

			var teamResult = await teamReadRepository.FindByIdAsync(domainEvent.TeamId);
			if (!teamResult.IsSuccess) {
				logger.LogError("Error from TeamReadSideRepository: {Error} when searching for {Id}", teamResult.Error, domainEvent.TeamId);
				queueService.Enqueue(domainEvent);
				return;
			}

			var team = teamResult.Value;
			team.ProjectIds.AddIfNotExists(domainEvent.Id);
			var updateTeamResult = await teamReadRepository.UpdateAsync(team.Id, team);
			if (!updateTeamResult.IsSuccess) {
				logger.LogError("Error from TeamReadSideRepository: {Error} when updating entity with {Id}", updateTeamResult.Error, team.Id);
				queueService.Enqueue(domainEvent);
				return;
			}

			var projectResult = await projectReadRepository.AddAsync(mapper.Map<ProjectReadModel>(domainEvent));
			if (!projectResult.IsSuccess) {
				logger.LogError("Error from ProjectReadSideRepository: {Error} when adding entity with {Id}", projectResult.Error, domainEvent.Id);
				queueService.Enqueue(domainEvent);
			}
		}
	}
}
