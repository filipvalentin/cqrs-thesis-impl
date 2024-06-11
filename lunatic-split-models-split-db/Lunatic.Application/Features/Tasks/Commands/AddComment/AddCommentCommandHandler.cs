using AutoMapper;
using Lunatic.Application.Features.Comments.Payload;
using Lunatic.Application.Persistence.WriteSide;
using Lunatic.Domain.DomainEvents.Comment;
using Lunatic.Domain.Entities;
using MediatR;

namespace Lunatic.Application.Features.Tasks.Commands.CreateComment {
	public class AddCommentCommandHandler : IRequestHandler<AddCommentCommand, AddCommentCommandResponse> {
		private readonly ITaskRepository taskRepository;
		private readonly ICommentRepository commentRepository;
		private readonly IUserRepository userRepository;
		private readonly IPublisher publisher;
		private readonly IMapper mapper;

		public AddCommentCommandHandler(ITaskRepository taskRepository, ICommentRepository commentRepository,
			IUserRepository userRepository, IPublisher publisher, IMapper mapper) {
			this.taskRepository = taskRepository;
			this.commentRepository = commentRepository;
			this.userRepository = userRepository;
			this.publisher = publisher;
			this.mapper = mapper;
		}

		public async Task<AddCommentCommandResponse> Handle(AddCommentCommand request, CancellationToken cancellationToken) {
			var validator = new AddCommentCommandValidator(userRepository, taskRepository);
			var validatorResult = await validator.ValidateAsync(request, cancellationToken);

			if (!validatorResult.IsValid) {
				return new AddCommentCommandResponse {
					Success = false,
					ValidationErrors = validatorResult.Errors.Select(e => e.ErrorMessage).ToList()
				};
			}

			var commentResult = Comment.Create(request.UserId, request.TaskId, request.Content);
			if (!commentResult.IsSuccess) {
				return new AddCommentCommandResponse {
					Success = false,
					ValidationErrors = new List<string> { commentResult.Error }
				};
			}

			var task = (await taskRepository.FindByIdAsync(request.TaskId)).Value;
			task.AddComment(commentResult.Value);
			await taskRepository.UpdateAsync(task);

			await commentRepository.AddAsync(commentResult.Value);

			await publisher.Publish(mapper.Map<CommentAddedDomainEvent>(commentResult.Value), cancellationToken);

			return new AddCommentCommandResponse {
				Success = true,
				Comment = mapper.Map<CommentDto>(commentResult.Value)
			};
		}
	}
}
