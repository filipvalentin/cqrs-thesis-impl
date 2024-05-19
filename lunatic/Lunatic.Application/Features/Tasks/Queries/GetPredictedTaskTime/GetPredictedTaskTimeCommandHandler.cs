using Lunatic.Application.Persistence;
using MediatR;


namespace Lunatic.Application.Features.Tasks.Queries.GetPredictedTaskTime {
	public class GetPredictedTaskTimeCommandHandler : IRequestHandler<GetPredictedTaskTimeCommand, GetPredictedTaskTimeCommandResponse> {
		private readonly ITaskRepository taskRepository;

		private readonly ICommentRepository commentRepository;

		public GetPredictedTaskTimeCommandHandler(ITaskRepository taskRepository, ICommentRepository commentRepository) {
			this.taskRepository = taskRepository;
			this.commentRepository = commentRepository;
		}

		public async Task<GetPredictedTaskTimeCommandResponse> Handle(GetPredictedTaskTimeCommand request, CancellationToken cancellationToken) {
			var validator = new GetPredictedTaskTimeCommandValidator(taskRepository);
			var validatorResult = await validator.ValidateAsync(request, cancellationToken);

			if (!validatorResult.IsValid) {
				return new GetPredictedTaskTimeCommandResponse {
					Success = false,
					ValidationErrors = validatorResult.Errors.Select(e => e.ErrorMessage).ToList()
				};
			}

			//var task = (await this.taskRepository.FindByUserIdAsync(request.TaskId)).Value;

			//var commentIds = task.CommentIds;
			//var comments = new List<Comment>();
			//float averageCommentsLength = 0;
			//foreach (var commentId in commentIds) {
			//    var comment = (await this.commentRepository.FindByUserIdAsync(commentId)).Value;
			//    averageCommentsLength += comment.Content.Length;
			//    comments.Add(comment);
			//}
			//if(comments.Count > 0) {
			//    averageCommentsLength /= comments.Count;
			//}

			//DaysPrediction.ModelInput data = new DaysPrediction.ModelInput() {
			//    People_Assigned = task.AssigneeIds.Count,
			//    Description__characters_ = task.Description.Length,
			//    Comments_per_task = task.CommentIds.Count,
			//    Comment_average_length = averageCommentsLength,
			//    Priority = (int)task.Priority
			//};

			//var prediction = DaysPrediction.Predict(data);

			return new GetPredictedTaskTimeCommandResponse {
				Success = true,
				Duration = 44//prediction.Score
			};
		}
	}
}
