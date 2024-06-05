using AutoMapper;
using Lunatic.Application.Persistence.ReadSide;
using Lunatic.Application.Persistence.WriteSide;
using Lunatic.Domain.DomainEvents.User;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Lunatic.Application.Features.Users.Events {
	public class UserDeletedDomainEventHandler : INotificationHandler<UserDeletedDomainEvent> {
		private readonly IUserReadSideRepository userReadRepository;
		private readonly ILogger<UserDeletedDomainEventHandler> logger;

		public UserDeletedDomainEventHandler(IUserReadSideRepository userReadRepository, ILogger<UserDeletedDomainEventHandler> logger) {
			this.userReadRepository = userReadRepository;
			this.logger = logger;
		}

		public async Task Handle(UserDeletedDomainEvent notification, CancellationToken cancellationToken) {
			var status = await userReadRepository.DeleteAsync(notification.Id);

			if (!status.IsSuccess) {
				logger.LogError("Error while deleting user with id {Id} from read side", notification.Id);
			}
		}
	}
}
