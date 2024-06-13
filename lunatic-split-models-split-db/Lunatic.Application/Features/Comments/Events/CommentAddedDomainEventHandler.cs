using AutoMapper;
using Lunatic.Application.Models.ReadModels;
using Lunatic.Application.Persistence.ReadSide;
using Lunatic.Application.Utils.Services;
using Lunatic.Domain.DomainEvents.Comment;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Lunatic.Application.Features.Comments.Events {
	public class CommentAddedDomainEventHandler(
		ICommentReadSideRepository commentReadRepository,
		ILogger<CommentAddedDomainEventHandler> logger, 
		IMapper mapper, 
		IEventQueueService queueService) : INotificationHandler<CommentAddedDomainEvent> {

		private readonly ICommentReadSideRepository commentReadRepository = commentReadRepository;
		private readonly ILogger<CommentAddedDomainEventHandler> logger = logger;
		private readonly IMapper mapper = mapper;
		private readonly IEventQueueService queueService = queueService;

		public async Task Handle(CommentAddedDomainEvent domainEvent, CancellationToken cancellationToken) {
			var status = await commentReadRepository.AddAsync(mapper.Map<CommentReadModel>(domainEvent));
			if (!status.IsSuccess) {
				logger.LogError("Error from CommentReadSideRepository: {Error} when adding entity with {Id}", status.Error, domainEvent.Id);
				queueService.Enqueue(domainEvent);
			}
		}
	}
}
