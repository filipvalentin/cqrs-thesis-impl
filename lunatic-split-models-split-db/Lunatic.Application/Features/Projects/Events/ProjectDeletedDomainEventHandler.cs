using Lunatic.Application.Persistence.ReadSide;
using Lunatic.Application.Persistence.WriteSide;
using Lunatic.Domain.DomainEvents.Project;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Lunatic.Application.Features.Projects.Events {
	public class ProjectDeletedDomainEventHandler : INotificationHandler<ProjectDeletedDomainEvent>{
		private readonly IProjectReadSideRepository projectReadRepository;
		private readonly ITaskRepository taskWriteRepository;
		private readonly ILogger<ProjectDeletedDomainEventHandler> logger;
		private readonly IPublisher publisher;

		public ProjectDeletedDomainEventHandler(IProjectReadSideRepository projectReadRepository, ITaskRepository taskWriteRepository, 
			ILogger<ProjectDeletedDomainEventHandler> logger, IPublisher publisher) {
			this.projectReadRepository = projectReadRepository;
			this.taskWriteRepository = taskWriteRepository;
			this.logger = logger;
			this.publisher = publisher;
		}

		public async Task Handle(ProjectDeletedDomainEvent notification, CancellationToken cancellationToken) {
			throw new NotImplementedException();
		}
	}
}
