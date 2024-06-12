using AutoMapper;
using Lunatic.Application.Models.ReadModels;
using Lunatic.Application.Persistence.ReadSide;
using Lunatic.Application.Persistence.WriteSide;
using Lunatic.Application.Utils.Services;
using Lunatic.Domain.DomainEvents.User;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Lunatic.Application.Features.Users.Events {
	public class UserUpdatedDomainEventHandler(
		IUserReadSideRepository userReadRepository,
		IUserRepository userWriteRepository, 
		ILogger<UserUpdatedDomainEventHandler> logger, 
		IMapper mapper, 
		IEventQueueService queueService) : INotificationHandler<UserUpdatedDomainEvent> {
		
		private readonly IUserReadSideRepository userReadRepository = userReadRepository;
		private readonly IUserRepository userWriteRepository = userWriteRepository;
		private readonly ILogger<UserUpdatedDomainEventHandler> logger = logger;
		private readonly IMapper mapper = mapper;
		private readonly IEventQueueService queueService = queueService;

		public async Task Handle(UserUpdatedDomainEvent domainEvent, CancellationToken cancellationToken) {
			var updateResult = await userReadRepository.UpdateAsync(domainEvent.Id, mapper.Map<UserReadModel>(domainEvent));
			if (!updateResult.IsSuccess) {
				logger.LogError("Failed to update user in read side repository. UserId: {UserId}", domainEvent.Id);
				queueService.Enqueue(domainEvent);
			}
		}
	}
}
