using Lunatic.Application.Persistence.ReadSide;
using Lunatic.Application.Persistence.WriteSide;
using Lunatic.Domain.DomainEvents.Project;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Lunatic.Application.Features.Projects.Events {
	public class ProjectDeletedDomainEventHandler(IProjectReadSideRepository projectReadRepository, ITaskRepository taskWriteRepository,
		ILogger<ProjectDeletedDomainEventHandler> logger, IPublisher publisher) : INotificationHandler<ProjectDeletedDomainEvent>{
		private readonly IProjectReadSideRepository projectReadRepository = projectReadRepository;
		private readonly ITaskRepository taskWriteRepository = taskWriteRepository;
		private readonly ILogger<ProjectDeletedDomainEventHandler> logger = logger;
		private readonly IPublisher publisher = publisher;

		public async Task Handle(ProjectDeletedDomainEvent notification, CancellationToken cancellationToken) {
			
			
		}
	}
}
