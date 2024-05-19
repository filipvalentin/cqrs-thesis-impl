using Lunatic.UI.Models.Dtos;
using Lunatic.UI.Models.ViewModels;
using Lunatic.UI.Services.Responses;

namespace Lunatic.UI.Contracts {
	public interface IProjectDataService {
		Task<ApiResponse<TaskDto>> CreateTaskAsync(CreateTaskDto taskViewModel);
		Task<ApiResponse<List<TaskDto>>> GetProjectTasksAsync(Guid projectId);
		Task<ApiResponse> EditProjectInfoAsync(Guid projectId, EditProjectDto project);
		Task<ApiResponse<ProjectDto>> GetProjectByIdAsync(Guid teamId);
		Task<ApiResponse> DeleteTaskAsync(Guid projectId, Guid taskId);
		Task<ApiResponse> AddSectionAsync(Guid projectId, string sectionTitle);
		Task<ApiResponse> RenameSectionAsync(Guid projectId, string oldSection, string newSection);
		Task<ApiResponse> DeleteSectionAsync(Guid projectId, string sectionTitle);
	}
}

