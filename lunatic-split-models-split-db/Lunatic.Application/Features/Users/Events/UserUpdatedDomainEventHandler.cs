using AutoMapper;
using Lunatic.Application.Models.ReadModels;
using Lunatic.Application.Persistence.ReadSide;
using Lunatic.Application.Persistence.WriteSide;
using Lunatic.Domain.DomainEvents.User;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Lunatic.Application.Features.Users.Events {
	public class UserUpdatedDomainEventHandler : INotificationHandler<UserUpdatedDomainEvent> {
		private readonly IUserReadSideRepository userReadRepository;
		private readonly IUserRepository userWriteRepository;
		private readonly ILogger<UserUpdatedDomainEventHandler> logger;
		private readonly IMapper mapper;

		public UserUpdatedDomainEventHandler(IUserReadSideRepository userReadRepository,
			IUserRepository userWriteRepository, ILogger<UserUpdatedDomainEventHandler> logger, IMapper mapper) {
			this.userReadRepository = userReadRepository;
			this.userWriteRepository = userWriteRepository;
			this.logger = logger;
			this.mapper = mapper;
		}

		public async Task Handle(UserUpdatedDomainEvent notification, CancellationToken cancellationToken) {
			var userResult = await userWriteRepository.FindByIdAsync(notification.Id);

			if (!userResult.IsSuccess) {
				logger.LogError("User not found in write side repository. UserId: {UserId}", notification.Id);
				return;
			}

			var updateResult = await userReadRepository.UpdateAsync(notification.Id, mapper.Map<UserReadModel>(userResult.Value));

			if (!updateResult.IsSuccess) {
				logger.LogError("Failed to update user in read side repository. UserId: {UserId}", notification.Id);
			}
		}
	}
}
