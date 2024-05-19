using Lunatic.UI.Models.Dtos;
using Lunatic.UI.Models.ViewModels;
using Lunatic.UI.Services.Responses;

namespace Lunatic.UI.Contracts {
	public interface ITaskDataService {
		Task<ApiResponse<CommentDto>> AddCommentAsync(Guid taskId, Guid userId, string comment);
		Task<ApiResponse<TaskDto>> GetTaskByIdAsync(Guid taskId);
		Task<ApiResponse<TaskDto>> EditTaskInfoAsync(Guid taskId, EditTaskViewModel task);
		Task<ApiResponse> UpdateTaskSectionAsync(Guid taskId, string taskSection);
		Task<ApiResponse> UpdateTaskStatusAsync(Guid taskId, Models.Shared.TaskStatus taskStatus);
		Task<ApiResponse> DeleteCommentAsync(Guid taskId, Guid commentId);
		Task<ApiResponse<decimal>> GetTaskPredictedDurationAsync(Guid taskId);
	}
}
