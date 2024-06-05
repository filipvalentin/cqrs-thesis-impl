﻿using Lunatic.Application.Persistence.ReadSide;
using Lunatic.Application.Persistence.WriteSide;
using Lunatic.Domain.DomainEvents.User;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Lunatic.Application.Features.Users.Events {
	public class UserCreatedDomainEventHandler : INotificationHandler<UserCreatedDomainEvent> {
		IUserReadSideRepository userReadRepository;
		IUserRepository userWriteRepository;
		ILogger<UserCreatedDomainEventHandler> logger;

		public UserCreatedDomainEventHandler(IUserReadSideRepository userReadRepository, IUserRepository userWriteRepository, ILogger<UserCreatedDomainEventHandler> logger) {
			this.userReadRepository = userReadRepository;
			this.userWriteRepository = userWriteRepository;
			this.logger = logger;
		}

		public async Task Handle(UserCreatedDomainEvent notification, CancellationToken cancellationToken) {
			
		}
	}
}