using Lunatic.Application.Features.Tasks.Interfaces;
using Lunatic.Application.Persistence;
using Lunatic.Domain.Entities;
using Lunatic.Domain.MLModel;
using MediatR;
using Task = System.Threading.Tasks.Task;


namespace Lunatic.Application.Features.Tasks.Queries.GetPredictedTaskTime {
	public class GetPredictedTaskTimeCommandHandler : IRequestHandler<GetPredictedTaskTimeCommand, GetPredictedTaskTimeCommandResponse> {
		private readonly ITaskRepository taskRepository;
		private readonly ICommentRepository commentRepository;
		private readonly IMLDataProvider mLDataProvider;

		public GetPredictedTaskTimeCommandHandler(ITaskRepository taskRepository, ICommentRepository commentRepository,
			IMLDataProvider mLDataProvider) {
			this.taskRepository = taskRepository;
			this.commentRepository = commentRepository;
			this.mLDataProvider = mLDataProvider;
		}

		public async Task<GetPredictedTaskTimeCommandResponse> Handle(GetPredictedTaskTimeCommand request, CancellationToken cancellationToken) {

			var taskResult = await taskRepository.FindByIdAsync(request.TaskId);
			if (taskResult == null) {
				return new GetPredictedTaskTimeCommandResponse {
					Success = false,
					Message = "Task not found"
				};
			}

			var task = taskResult.Value;

			List<Comment> comments = new List<Comment>();
			foreach (var commentId in task.CommentIds) {
				var commentResult = await commentRepository.FindByIdAsync(commentId);
				if (commentResult.IsSuccess) {
					comments.Add(commentResult.Value);
				}
			}



			var predictionInputData = new DaysToCompleteTaskEntry {
				TaskId = task.Id.ToString(),
				AssigneesCount = task.AssigneeIds.Count,
				DescriptionLength = task.Description.Length,
				CommentsCount = comments.Count,
				AverageCommentLength = comments.Count > 0 ? comments.Average(comment => comment.Content.Length) : 0,
				Priority = task.Priority,
				ExpectedDaysToComplete = (task.PlannedEndDate - task.PlannedStartDate).Days
			};

			try {
				var prediction = await mLDataProvider.GetTaskPrediction(predictionInputData);
				return new GetPredictedTaskTimeCommandResponse {
					Success = true,
					Duration = prediction
				};
			}
			catch (Exception ex) {
				return new GetPredictedTaskTimeCommandResponse {
					Success = false,
					Message = $"An error occurred while predicting the task duration: {ex.Message}"
				};
			}
		}
	}
}
