using Lunatic.Application.Persistence.ReadSide;
using Lunatic.Application.Persistence.WriteSide;
using Lunatic.Domain.DomainEvents.User;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Lunatic.Application.Features.Users.Events {
	public class UserUpdatedDomainEventHandler : INotificationHandler<UserUpdatedDomainEvent> {
		IUserReadSideRepository userReadRepository;
		IUserRepository userWriteRepository;
		ILogger<UserUpdatedDomainEventHandler> logger;

		public UserUpdatedDomainEventHandler(IUserReadSideRepository userReadRepository,
			IUserRepository userWriteRepository, ILogger<UserUpdatedDomainEventHandler> logger) {
			this.userReadRepository = userReadRepository;
			this.userWriteRepository = userWriteRepository;
			this.logger = logger;
		}

		public async Task Handle(UserUpdatedDomainEvent notification, CancellationToken cancellationToken) {

		}
	}
}
