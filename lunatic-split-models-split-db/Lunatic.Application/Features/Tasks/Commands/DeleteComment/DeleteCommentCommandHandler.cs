using Lunatic.Application.Persistence.WriteSide;
using Lunatic.Domain.DomainEvents.Comment;
using MediatR;

namespace Lunatic.Application.Features.Tasks.Commands.DeleteComment {
	public class DeleteTaskCommentCommandHandler : IRequestHandler<DeleteCommentCommand, DeleteCommentCommandResponse> {
		private readonly ITaskRepository taskRepository;
		private readonly ICommentRepository commentRepository;
		private readonly IPublisher publisher;

		public DeleteTaskCommentCommandHandler(ITaskRepository taskRepository, ICommentRepository commentRepository, IPublisher publisher) {
			this.taskRepository = taskRepository;
			this.commentRepository = commentRepository;
			this.publisher = publisher;
		}

		public async Task<DeleteCommentCommandResponse> Handle(DeleteCommentCommand request, CancellationToken cancellationToken) {
			var taskResult = await taskRepository.FindByIdAsync(request.TaskId);
			if (!taskResult.IsSuccess) {
				return new DeleteCommentCommandResponse {
					Success = false,
					Message = taskResult.Error
				};
			}

			var task = taskResult.Value;
			task.RemoveComment(request.CommentId);
			var updateResult = await taskRepository.UpdateAsync(task);
			if (!updateResult.IsSuccess) {
				return new DeleteCommentCommandResponse {
					Success = false,
					Message = updateResult.Error
				};
			}

			var result = await commentRepository.DeleteAsync(request.CommentId);
			if (!result.IsSuccess) {
				return new DeleteCommentCommandResponse {
					Success = false,
					ValidationErrors = new List<string> { result.Error }
				};
			}

			await publisher.Publish(new CommentDeletedDomainEvent(Id: request.CommentId, Cascaded: false, TaskId: request.TaskId), cancellationToken);

			return new DeleteCommentCommandResponse {
				Success = true
			};
		}
	}
}
