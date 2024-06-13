using AutoMapper;
using Lunatic.Application.Persistence.WriteSide;
using Lunatic.Domain.DomainEvents.Comment;
using Lunatic.Domain.DomainEvents.Task;
using MediatR;

namespace Lunatic.Application.Features.Tasks.Commands.DeleteComment {
	public class DeleteTaskCommentCommandHandler(
		ITaskRepository taskRepository,
		ICommentRepository commentRepository,
		IPublisher publisher,
		IMapper mapper,
		IUnitOfWork unitOfWork) : IRequestHandler<DeleteCommentCommand, DeleteCommentCommandResponse> {

		private readonly ITaskRepository taskRepository = taskRepository;
		private readonly ICommentRepository commentRepository = commentRepository;
		private readonly IPublisher publisher = publisher;
		private readonly IMapper mapper = mapper;
		private readonly IUnitOfWork unitOfWork = unitOfWork;

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

			await unitOfWork.SaveChangesAsync(cancellationToken);

			await publisher.Publish(mapper.Map<TaskUpdatedDomainEvent>(task), cancellationToken);
			await publisher.Publish(new CommentDeletedDomainEvent(request.CommentId), cancellationToken);

			return new DeleteCommentCommandResponse {
				Success = true
			};
		}
	}
}
