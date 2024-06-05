using AutoMapper;
using Lunatic.Application.Models.ReadModels;
using Lunatic.Application.Persistence.ReadSide;
using Lunatic.Application.Persistence.WriteSide;
using Lunatic.Domain.DomainEvents.User;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Lunatic.Application.Features.Users.Events {
	public class UserCreatedDomainEventHandler : INotificationHandler<UserCreatedDomainEvent> {
		private readonly IUserReadSideRepository userReadRepository;
		private readonly IUserRepository userWriteRepository;
		private readonly ILogger<UserCreatedDomainEventHandler> logger;
		private readonly IMapper mapper;

		public UserCreatedDomainEventHandler(IUserReadSideRepository userReadRepository, IUserRepository userWriteRepository,
			ILogger<UserCreatedDomainEventHandler> logger, IMapper mapper) {
			this.userReadRepository = userReadRepository;
			this.userWriteRepository = userWriteRepository;
			this.logger = logger;
			this.mapper = mapper;
		}

		public async Task Handle(UserCreatedDomainEvent notification, CancellationToken cancellationToken) {
			var userResult = await userWriteRepository.FindByIdAsync(notification.Id);

			if (!userResult.IsSuccess) {
				logger.LogError("User with id {Id} not found", notification.Id);
				return;
			}

			var status = await userReadRepository.AddAsync(mapper.Map<UserReadModel>(userResult.Value));

			if (!status.IsSuccess) {
				logger.LogError("Error while adding user with id {Id} to read side", notification.Id);
			}
		}
	}
}
