using AutoMapper;
using Lunatic.Application.Features.Comments.Payload;
using Lunatic.Application.Persistence.WriteSide;
using MediatR;


namespace Lunatic.Application.Features.Comments.Commands.UpdateComment {
	public class UpdateCommentCommandHandler : IRequestHandler<UpdateCommentCommand, UpdateTaskCommentCommandResponse> {
		private readonly ICommentRepository commentRepository;
		private readonly IMapper mapper;
		private readonly IPublisher publisher;

		public UpdateCommentCommandHandler(ICommentRepository commentRepository, IMapper mapper, IPublisher publisher) {
			this.commentRepository = commentRepository;
			this.mapper = mapper;
			this.publisher = publisher;
		}

		public async Task<UpdateTaskCommentCommandResponse> Handle(UpdateCommentCommand request, CancellationToken cancellationToken) {
			//var validator = new UpdateCommentCommandValidator(commentRepository);
			//var validatorResult = await validator.ValidateAsync(request, cancellationToken);

			//if (!validatorResult.IsValid) {
			//	return new UpdateTaskCommentCommandResponse {
			//		Success = false,
			//		ValidationErrors = validatorResult.Errors.Select(e => e.ErrorMessage).ToList()
			//	};
			//}

			var commentResult = await commentRepository.FindByIdAsync(request.CommentId);

			var comment = commentResult.Value;

			comment.Update(request.Content);

			var dbCommentResult = await commentRepository.UpdateAsync(comment);

			if (!dbCommentResult.IsSuccess) {
				return new UpdateTaskCommentCommandResponse {
					Success = false,
					ValidationErrors = new List<string> { "Error updating comment" }
				};
			}

			await publisher.Publish(new CommentUpdatedDomainEvent(comment.CommentId), cancellationToken);

			return new UpdateTaskCommentCommandResponse {
				Success = true,
				Comment = mapper.Map<CommentDto>(comment)
			};
		}
	}
}
