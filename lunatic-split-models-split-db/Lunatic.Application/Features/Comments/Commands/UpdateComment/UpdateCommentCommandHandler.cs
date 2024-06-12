using AutoMapper;
using Lunatic.Application.Features.Comments.Payload;
using Lunatic.Application.Persistence.WriteSide;
using Lunatic.Domain.DomainEvents.Comment;
using MediatR;


namespace Lunatic.Application.Features.Comments.Commands.UpdateComment {
	public class UpdateCommentCommandHandler(
		ICommentRepository commentRepository, 
		IMapper mapper,
		IPublisher publisher) : IRequestHandler<UpdateCommentCommand, UpdateTaskCommentCommandResponse> {

		private readonly ICommentRepository commentRepository = commentRepository;
		private readonly IMapper mapper = mapper;
		private readonly IPublisher publisher = publisher;

		public async Task<UpdateTaskCommentCommandResponse> Handle(UpdateCommentCommand request, CancellationToken cancellationToken) {

			var commentResult = await commentRepository.FindByIdAsync(request.CommentId);
			if (!commentResult.IsSuccess) {
				return new UpdateTaskCommentCommandResponse {
					Success = false,
					Message = commentResult.Error
				};
			}

			var comment = commentResult.Value;
			comment.Update(request.Content);
			var dbCommentResult = await commentRepository.UpdateAsync(comment);
			if (!dbCommentResult.IsSuccess) {
				return new UpdateTaskCommentCommandResponse {
					Success = false,
					Message = dbCommentResult.Error
				};
			}

			await publisher.Publish(mapper.Map<CommentEditedDomainEvent>(comment), cancellationToken);

			return new UpdateTaskCommentCommandResponse {
				Success = true,
				Comment = mapper.Map<CommentDto>(comment)
			};
		}
	}
}
