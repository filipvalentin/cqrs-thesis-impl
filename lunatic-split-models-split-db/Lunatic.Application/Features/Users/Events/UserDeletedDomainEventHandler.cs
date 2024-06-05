using Lunatic.Application.Persistence.ReadSide;
using Lunatic.Application.Persistence.WriteSide;
using Lunatic.Domain.DomainEvents.User;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Lunatic.Application.Features.Users.Events {
	public class UserDeletedDomainEventHandler : INotificationHandler<UserDeletedDomainEvent> {
		IUserReadSideRepository userReadRepository;
		IUserRepository userWriteRepository;
		ILogger<UserDeletedDomainEventHandler> logger;

		public UserDeletedDomainEventHandler(IUserReadSideRepository userReadRepository, IUserRepository userWriteRepository, 
			ILogger<UserDeletedDomainEventHandler> logger) {
			this.userReadRepository = userReadRepository;
			this.userWriteRepository = userWriteRepository;
			this.logger = logger;
		}

		public async Task Handle(UserDeletedDomainEvent notification, CancellationToken cancellationToken) {
			
		}
	}
}
