using AutoMapper;
using Lunatic.Application.Models.ReadModels;
using Lunatic.Application.Persistence.ReadSide;
using Lunatic.Application.Persistence.WriteSide;
using Lunatic.Domain.DomainEvents.Comment;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Lunatic.Application.Features.Comments.Events {
	public class CommentCreatedDomainEventHandler : INotificationHandler<CommentCreatedDomainEvent> {
		ICommentReadSideRepository commentReadRepository;
		ICommentRepository commentWriteRepository;
		ILogger<CommentCreatedDomainEventHandler> logger;
		private readonly IMapper mapper;

		public CommentCreatedDomainEventHandler(ICommentReadSideRepository commentReadRepository,
			ICommentRepository commentWriteRepository, ILogger<CommentCreatedDomainEventHandler> logger, IMapper mapper) {
			this.commentReadRepository = commentReadRepository;
			this.commentWriteRepository = commentWriteRepository;
			this.logger = logger;
			this.mapper = mapper;
		}

		public async Task Handle(CommentCreatedDomainEvent notification, CancellationToken cancellationToken) {
			var commentResult = await commentWriteRepository.FindByIdAsync(notification.Id);

			if (!commentResult.IsSuccess) {
				logger.LogError($"Comment with id {notification.Id} not found");
				return;
			}

			var comment = commentResult.Value;

			var status = await commentReadRepository.AddAsync(mapper.Map<CommentReadModel>(comment));

			if (!status.IsSuccess) {
				logger.LogError($"Error adding comment with id {notification.Id} to read side");
			}

		}
	}
}
