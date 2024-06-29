using AutoMapper;
using Lunatic.Application.Features.Comments.Payload;
using Lunatic.Application.Persistence.WriteSide;
using Lunatic.Domain.DomainEvents.Comment;
using MediatR;

namespace Lunatic.Application.Features.Comments.Commands.EditComment {
	public class EditCommentCommandHandler(
		ICommentRepository commentRepository,
		IMapper mapper,
		IPublisher publisher,
		IUnitOfWork unitOfWork) : IRequestHandler<EditCommentCommand, EditCommentCommandResponse> {

		private readonly ICommentRepository commentRepository = commentRepository;
		private readonly IMapper mapper = mapper;
		private readonly IPublisher publisher = publisher;
		private readonly IUnitOfWork unitOfWork = unitOfWork;

		public async Task<EditCommentCommandResponse> Handle(EditCommentCommand request, CancellationToken cancellationToken) {

			var commentResult = await commentRepository.FindByIdAsync(request.CommentId);
			if (!commentResult.IsSuccess) {
				return new EditCommentCommandResponse {
					Success = false,
					Message = commentResult.Error
				};
			}

			var comment = commentResult.Value;
			comment.Update(request.Content);
			var dbCommentResult = await commentRepository.UpdateAsync(comment);
			if (!dbCommentResult.IsSuccess) {
				return new EditCommentCommandResponse {
					Success = false,
					Message = dbCommentResult.Error
				};
			}

			await unitOfWork.SaveChangesAsync(cancellationToken);

			await publisher.Publish(new CommentEditedDomainEvent(
				Id : comment.CommentId,
				TaskId : comment.TaskId,
				CreatedByUserId : comment.CreatedByUserId,
				Content : comment.Content,
				CreatedDate : comment.CreatedDate,
				LastModifiedDate : comment.LastModifiedDate
			), cancellationToken);

			return new EditCommentCommandResponse {
				Success = true,
				Comment = mapper.Map<CommentDto>(comment)
			};
		}
	}
}
