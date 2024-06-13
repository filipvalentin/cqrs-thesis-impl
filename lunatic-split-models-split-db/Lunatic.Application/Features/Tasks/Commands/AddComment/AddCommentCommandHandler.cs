using AutoMapper;
using Lunatic.Application.Features.Comments.Payload;
using Lunatic.Application.Persistence.WriteSide;
using Lunatic.Domain.DomainEvents.Comment;
using Lunatic.Domain.DomainEvents.Task;
using Lunatic.Domain.Entities;
using MediatR;

namespace Lunatic.Application.Features.Tasks.Commands.CreateComment {
	public class AddCommentCommandHandler(
		ITaskRepository taskRepository,
		ICommentRepository commentRepository,
		IUserRepository userRepository,
		IPublisher publisher,
		IMapper mapper) : IRequestHandler<AddCommentCommand, AddCommentCommandResponse> {

		private readonly ITaskRepository taskRepository = taskRepository;
		private readonly ICommentRepository commentRepository = commentRepository;
		private readonly IUserRepository userRepository = userRepository;
		private readonly IPublisher publisher = publisher;
		private readonly IMapper mapper = mapper;

		public async Task<AddCommentCommandResponse> Handle(AddCommentCommand request, CancellationToken cancellationToken) {
			var commentResult = Comment.Create(request.UserId, request.TaskId, request.Content);
			if (!commentResult.IsSuccess) {
				return new AddCommentCommandResponse {
					Success = false,
					Message = commentResult.Error
				};
			}
			var comment = commentResult.Value;

			var taskResult = await taskRepository.FindByIdAsync(request.TaskId);
			if (!taskResult.IsSuccess) {
				return new AddCommentCommandResponse {
					Success = false,
					Message = taskResult.Error
				};
			}
			var task = taskResult.Value;

			task.AddComment(comment.CommentId);
			var taskUpdatedResult = await taskRepository.UpdateAsync(task);
			if (!taskUpdatedResult.IsSuccess) {
				return new AddCommentCommandResponse {
					Success = false,
					Message = taskUpdatedResult.Error
				};
			}

			var commentAddedResult = await commentRepository.AddAsync(comment);
			if (!commentAddedResult.IsSuccess) {
				return new AddCommentCommandResponse {
					Success = false,
					Message = commentAddedResult.Error
				};
			}

			await publisher.Publish(mapper.Map<TaskUpdatedDomainEvent>(task), cancellationToken);
			await publisher.Publish(
				new CommentAddedDomainEvent(
					Id: comment.CommentId,
					CreatedByUserId: comment.CreatedByUserId,
					TaskId: comment.TaskId,
					Content: comment.Content,
					CreatedDate: comment.CreatedDate,
					LastModifiedDate: comment.LastModifiedDate),
				cancellationToken);

			return new AddCommentCommandResponse {
				Success = true,
				Comment = mapper.Map<CommentDto>(comment)
			};
		}
	}
}
