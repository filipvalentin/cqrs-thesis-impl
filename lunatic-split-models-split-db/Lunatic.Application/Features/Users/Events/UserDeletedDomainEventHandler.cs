using Lunatic.Application.Persistence.ReadSide;
using Lunatic.Application.Utils.Services;
using Lunatic.Domain.DomainEvents.User;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Lunatic.Application.Features.Users.Events {
	public class UserDeletedDomainEventHandler(
		IUserReadSideRepository userReadRepository, 
		ILogger<UserDeletedDomainEventHandler> logger, 
		IEventQueueService queueService) : INotificationHandler<UserDeletedDomainEvent> {

		private readonly IUserReadSideRepository userReadRepository = userReadRepository;
		private readonly ILogger<UserDeletedDomainEventHandler> logger = logger;
		private readonly IEventQueueService queueService = queueService;

		public async Task Handle(UserDeletedDomainEvent domainEvent, CancellationToken cancellationToken) {
			var status = await userReadRepository.DeleteAsync(domainEvent.Id);
			if (!status.IsSuccess) {
				logger.LogError("Failed to delete user with id {Id} from read side. Error: {Error}", domainEvent.Id, status.Error);
				queueService.Enqueue(domainEvent);				
			}
		}
	}
}
