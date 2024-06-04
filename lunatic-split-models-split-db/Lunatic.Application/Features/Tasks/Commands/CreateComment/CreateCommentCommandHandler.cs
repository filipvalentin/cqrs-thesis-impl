using Lunatic.Application.Features.Comments.Payload;
using Lunatic.Application.Persistence.WriteSide;
using Lunatic.Domain.Entities;
using MediatR;



namespace Lunatic.Application.Features.Tasks.Commands.CreateComment
{
    public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, CreateCommentCommandResponse> {
		private readonly ITaskRepository taskRepository;

		private readonly ICommentRepository commentRepository;

		private readonly IUserRepository userRepository;

		public CreateCommentCommandHandler(ITaskRepository taskRepository, ICommentRepository commentRepository, IUserRepository userRepository) {
			this.taskRepository = taskRepository;
			this.commentRepository = commentRepository;
			this.userRepository = userRepository;
		}

		public async Task<CreateCommentCommandResponse> Handle(CreateCommentCommand request, CancellationToken cancellationToken) {
			var validator = new CreateCommentCommandValidator(userRepository, taskRepository);
			var validatorResult = await validator.ValidateAsync(request, cancellationToken);

			if (!validatorResult.IsValid) {
				return new CreateCommentCommandResponse {
					Success = false,
					ValidationErrors = validatorResult.Errors.Select(e => e.ErrorMessage).ToList()
				};
			}

			var commentResult = Comment.Create(request.UserId, request.TaskId, request.Content);
			if (!commentResult.IsSuccess) {
				return new CreateCommentCommandResponse {
					Success = false,
					ValidationErrors = new List<string> { commentResult.Error }
				};
			}

			var task = (await taskRepository.FindByIdAsync(request.TaskId)).Value;
			task.AddComment(commentResult.Value);
			await taskRepository.UpdateAsync(task);

			await commentRepository.AddAsync(commentResult.Value);

			return new CreateCommentCommandResponse {
				Success = true,
				Comment = new CommentDto {
					CommentId = commentResult.Value.CommentId,
					TaskId = commentResult.Value.TaskId,
					AuthorId = commentResult.Value.CreatedByUserId,

					Content = commentResult.Value.Content,

					//EmoteIds = commentResult.Value.EmoteIds,

					CreatedDate = commentResult.Value.CreatedDate,
					LastModifiedDate = commentResult.Value.LastModifiedDate
				}
			};
		}
	}
}
