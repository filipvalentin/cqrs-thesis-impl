using Lunatic.UI.Contracts;
using Lunatic.UI.Models.Dtos;
using Lunatic.UI.Models.ViewModels;
using Lunatic.UI.Services.Responses;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Lunatic.UI.Services {
	public class ProjectDataService : IProjectDataService {
		private const string RequestUri = "api/v1/projects";
		private readonly HttpClient httpClient;
		private readonly ITokenService tokenService;

		public ProjectDataService(HttpClient httpClient, ITokenService tokenService) {
			this.httpClient = httpClient;
			this.tokenService = tokenService;
		}

		public async Task<ApiResponse<TaskDto>> CreateTaskAsync(CreateTaskDto taskViewModel) {
			var result = await httpClient.PostAsJsonAsync($"{RequestUri}/{taskViewModel.ProjectId}/tasks", taskViewModel);
			var response = await result.Content.ReadFromJsonAsync<ApiResponse<TaskDto>>();
			response!.Success = result.IsSuccessStatusCode;
			return response!;
		}

		public async Task<ApiResponse> DeleteTaskAsync(Guid projectId, Guid taskId) {
			var result = await httpClient.DeleteAsync($"{RequestUri}/{projectId}/tasks/{taskId}");
			var response = await result.Content.ReadFromJsonAsync<ApiResponse>();
			response!.Success = result.IsSuccessStatusCode;
			return response!;
		}

		public async Task<ApiResponse> AddSectionAsync(Guid projectId, string sectionTitle) {
			var result = await httpClient.PostAsJsonAsync($"{RequestUri}/{projectId}/tasksections", new { ProjectId = projectId, Section = sectionTitle });
			var response = await result.Content.ReadFromJsonAsync<ApiResponse>();
			response!.Success = result.IsSuccessStatusCode;
			return response!;
		}

		[Obsolete("not used?")]
		public async Task<ApiResponse<ProjectDto>> CreateProjectAsync(ProjectViewModel projectViewModel) {
			httpClient.DefaultRequestHeaders.Authorization
				= new AuthenticationHeaderValue("Bearer", await tokenService.GetTokenAsync());
			var result = await httpClient.PostAsJsonAsync(RequestUri, projectViewModel);
			var response = await result.Content.ReadFromJsonAsync<ApiResponse<ProjectDto>>();
			response!.Success = result.IsSuccessStatusCode;
			return response!;
		}

		public async Task<ApiResponse> DeleteSectionAsync(Guid projectId, string sectionTitle) {
			var result = await httpClient.DeleteAsync($"{RequestUri}/{projectId}/tasksections/{sectionTitle}"); // TODO! check if PATCH solves this more easily
			var response = await result.Content.ReadFromJsonAsync<ApiResponse>();
			response!.Success = result.IsSuccessStatusCode;
			return response!;
		}

		public async Task<ApiResponse> EditProjectInfoAsync(Guid projectId, EditProjectDto project) {
			var result = await httpClient.PutAsJsonAsync($"{RequestUri}/{projectId}", project);
			var response = await result.Content.ReadFromJsonAsync<ApiResponse>();
			response!.Success = result.IsSuccessStatusCode;
			return response!;
		}

		public async Task<ApiResponse<ProjectDto>> GetProjectByIdAsync(Guid teamId) {
			var result = await httpClient.GetAsync($"{RequestUri}/{teamId}", HttpCompletionOption.ResponseHeadersRead);
			var response = await result.Content.ReadFromJsonAsync<ApiResponse<ProjectDto>>();
			response!.Success = result.IsSuccessStatusCode;
			return response!;
		}

		[Obsolete("not used?")]
		public async Task<ApiResponse<List<TaskDto>>> GetProjectTasksAsync(Guid projectId) {
			var result = await httpClient.GetAsync($"{RequestUri}/{projectId}/tasks", HttpCompletionOption.ResponseHeadersRead);
			var response = await result.Content.ReadFromJsonAsync<ApiResponse<List<TaskDto>>>();
			response!.Success = result.IsSuccessStatusCode;
			return response!;
		}

		public async Task<ApiResponse> RenameSectionAsync(Guid projectId, string oldSection, string newSection) {
			var result = await httpClient.PutAsJsonAsync($"{RequestUri}/{projectId}/tasksections/{oldSection}",
				new { ProjectId = projectId, Section = oldSection, NewSection = newSection });
			var response = await result.Content.ReadFromJsonAsync<ApiResponse>();
			response!.Success = result.IsSuccessStatusCode;
			return response!;
		}

	}
}
