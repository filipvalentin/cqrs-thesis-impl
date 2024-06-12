using AutoMapper;
using Lunatic.Application.Models.ReadModels;
using Lunatic.Application.Persistence.ReadSide;
using Lunatic.Application.Persistence.WriteSide;
using Lunatic.Application.Utils.Services;
using Lunatic.Domain.DomainEvents.User;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Lunatic.Application.Features.Users.Events {
	public class UserCreatedDomainEventHandler(
		IUserReadSideRepository userReadRepository,
		IUserRepository userWriteRepository,
		ILogger<UserCreatedDomainEventHandler> logger, 
		IMapper mapper, 
		IEventQueueService queueService) : INotificationHandler<UserCreatedDomainEvent> {

		private readonly IUserReadSideRepository userReadRepository = userReadRepository;
		private readonly IUserRepository userWriteRepository = userWriteRepository;
		private readonly ILogger<UserCreatedDomainEventHandler> logger = logger;
		private readonly IMapper mapper = mapper;
		private readonly IEventQueueService queueService = queueService;

		public async Task Handle(UserCreatedDomainEvent domainEvent, CancellationToken cancellationToken) {
			var status = await userReadRepository.AddAsync(mapper.Map<UserReadModel>(domainEvent));
			if (!status.IsSuccess) {
				logger.LogError("Failed to add user with id {Id} to read side. Error: {Error}", domainEvent.Id, status.Error);
				queueService.Enqueue(domainEvent);
			}
		}
	}
}
