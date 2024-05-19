using Lunatic.Application.Features.Comments.Payload;
using Lunatic.Application.Persistence;
using Lunatic.Domain.Entities;
using MediatR;


namespace Lunatic.Application.Features.Tasks.Queries.GetAllTaskComments {
	public class GetAllTaskCommentsQueryHandler : IRequestHandler<GetAllTaskCommentsQuery, GetAllTaskCommentsQueryResponse> {
		private readonly ITaskRepository taskRepository;

		private readonly ICommentRepository commentRepository;

		public GetAllTaskCommentsQueryHandler(ITaskRepository taskRepository, ICommentRepository commentRepository) {
			this.taskRepository = taskRepository;
			this.commentRepository = commentRepository;
		}

		public async Task<GetAllTaskCommentsQueryResponse> Handle(GetAllTaskCommentsQuery request, CancellationToken cancellationToken) {
			var taskResult = await taskRepository.FindByIdAsync(request.TaskId);
			if (!taskResult.IsSuccess) {
				return new GetAllTaskCommentsQueryResponse {
					Success = false,
					ValidationErrors = new List<string> { "Task not found" }
				};
			}

			GetAllTaskCommentsQueryResponse response = new GetAllTaskCommentsQueryResponse();
			var commentIds = taskResult.Value.CommentIds;
			var comments = new List<Comment>();
			foreach (var commentId in commentIds) {
				var comment = (await commentRepository.FindByIdAsync(commentId)).Value;
				comments.Add(comment);
			}

			response.Comments = comments.Select(comment => new CommentDto {
				CommentId = comment.CommentId,
				TaskId = comment.TaskId,
				AuthorId = comment.CreatedByUserId,

				Content = comment.Content,

				//EmoteIds = comment.EmoteIds,

				CreatedDate = comment.CreatedDate,
				LastModifiedDate = comment.LastModifiedDate
			}).ToList();
			return response;
		}
	}
}
