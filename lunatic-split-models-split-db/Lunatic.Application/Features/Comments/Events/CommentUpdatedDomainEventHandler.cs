using AutoMapper;
using Lunatic.Application.Models.ReadModels;
using Lunatic.Application.Persistence.ReadSide;
using Lunatic.Application.Persistence.WriteSide;
using Lunatic.Domain.DomainEvents.Comment;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Lunatic.Application.Features.Comments.Events {
	public class CommentUpdatedDomainEventHandler : INotificationHandler<CommentUpdatedDomainEvent> {
		private readonly ICommentRepository commentWriteRepository;
		private readonly ICommentReadSideRepository commentReadRepository;
		private readonly ILogger<CommentUpdatedDomainEventHandler> logger;
		private readonly IMapper mapper;

		public CommentUpdatedDomainEventHandler(ICommentRepository commentWriteRepository,
			ICommentReadSideRepository commentReadRepository, ILogger<CommentUpdatedDomainEventHandler> logger, IMapper mapper) {
			this.commentWriteRepository = commentWriteRepository;
			this.commentReadRepository = commentReadRepository;
			this.logger = logger;
			this.mapper = mapper;
		}

		public async Task Handle(CommentUpdatedDomainEvent notification, CancellationToken cancellationToken) {
			var commentResult = await commentWriteRepository.FindByIdAsync(notification.Id);

			if (!commentResult.IsSuccess) {
				logger.LogError("Comment with id {Id} not found", notification.Id);
				return;
			}

			var comment = commentResult.Value;

			var commentReadResult = await commentReadRepository.UpdateAsync(notification.Id, mapper.Map<CommentReadModel>(comment));

			if (!commentReadResult.IsSuccess) {
				logger.LogError("Comment with id {Id} not found", notification.Id);
			}
		}
	}
}
