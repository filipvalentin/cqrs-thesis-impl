using Lunatic.UI.Contracts;
using Lunatic.UI.Models.Dtos;
using Lunatic.UI.Models.ViewModels;
using Lunatic.UI.Services.Responses;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Lunatic.UI.Services {
	public class TeamDataService : ITeamDataService {
		private const string RequestUri = "api/v1/teams";
		private readonly HttpClient httpClient;
		private readonly ITokenService tokenService;

		public TeamDataService(HttpClient httpClient, ITokenService tokenService) {
			this.httpClient = httpClient;
			this.tokenService = tokenService;
		}

		public async Task<ApiResponse<TeamDto>> CreateTeamAsync(TeamViewModel teamViewModel) {
			httpClient.DefaultRequestHeaders.Authorization
				= new AuthenticationHeaderValue("Bearer", await tokenService.GetTokenAsync());
			var result = await httpClient.PostAsJsonAsync(RequestUri, teamViewModel);
			var response = await result.Content.ReadFromJsonAsync<ApiResponse<TeamDto>>();
			response!.Success = result.IsSuccessStatusCode;
			return response!;
		}

		[Obsolete("duplicate")]
		public async Task<ApiResponse> EditTeamInfoAsync(Guid teamId, EditTeamInfoViewModel editTeamInfoViewModel) {
			var result = await httpClient.PutAsJsonAsync($"{RequestUri}/{teamId}", editTeamInfoViewModel);
			var response = await result.Content.ReadFromJsonAsync<ApiResponse>();
			response!.Success = result.IsSuccessStatusCode;
			return response!;
		}

		public async Task<ApiResponse> DeleteTeamAsync(Guid teamId) {
			httpClient.DefaultRequestHeaders.Authorization
				= new AuthenticationHeaderValue("Bearer", await tokenService.GetTokenAsync());
			var result = await httpClient.DeleteAsync($"{RequestUri}/{teamId}");
			var response = await result.Content.ReadFromJsonAsync<ApiResponse>();
			response!.Success = result.IsSuccessStatusCode;
			return response!;
		}

		public async Task<ApiResponse<TeamDto>> GetTeamByIdAsync(Guid teamId) {
			var result = await httpClient.GetAsync($"{RequestUri}/{teamId}", HttpCompletionOption.ResponseHeadersRead);
			var team = await result.Content.ReadFromJsonAsync<ApiResponse<TeamDto>>();
			return team!;
		}

		public async Task<ApiResponse<List<UserDto>>> GetTeamMembersAsync(Guid teamId) {
			var result = await httpClient.GetAsync($"{RequestUri}/{teamId}/members", HttpCompletionOption.ResponseHeadersRead);
			var response = await result.Content.ReadFromJsonAsync<ApiResponse<List<UserDto>>>();
			response!.Success = result.IsSuccessStatusCode;
			return response!;
		}

		public async Task<ApiResponse> AddMemberToTeamAsync(Guid memberId, Guid teamId) {
			httpClient.DefaultRequestHeaders.Authorization
				= new AuthenticationHeaderValue("Bearer", await tokenService.GetTokenAsync());
			var result = await httpClient.PostAsJsonAsync($"{RequestUri}/{teamId}/members/",
				new AddTeamMemberDto() {
					UserId = memberId,
					TeamId = teamId
				});
			var response = await result.Content.ReadFromJsonAsync<ApiResponse>();
			response!.Success = result.IsSuccessStatusCode;
			return response!;
		}

		public async Task<ApiResponse<ProjectDto>> AddNewTeamProjectAsync(Guid teamId, ProjectViewModel projectViewModel) {
			httpClient.DefaultRequestHeaders.Authorization
				= new AuthenticationHeaderValue("Bearer", await tokenService.GetTokenAsync());
			var result = await httpClient.PostAsJsonAsync($"{RequestUri}/{teamId}/projects/", projectViewModel);
			var response = await result.Content.ReadFromJsonAsync<ApiResponse<ProjectDto>>();
			response!.Success = result.IsSuccessStatusCode;
			return response!;
		}

		public async Task<ApiResponse<TeamDto>> UpdateTeamInfoAsync(UpdateTeamInfoViewModel viewModel) {
			httpClient.DefaultRequestHeaders.Authorization
				= new AuthenticationHeaderValue("Bearer", await tokenService.GetTokenAsync());
			var result = await httpClient.PutAsJsonAsync($"{RequestUri}/{viewModel.TeamId}", viewModel);
			var response = await result.Content.ReadFromJsonAsync<ApiResponse<TeamDto>>();
			response!.Success = result.IsSuccessStatusCode;
			return response!;
		}

		public async Task<ApiResponse> DeleteProjectAsync(Guid teamId, Guid projectId) {
			var result = await httpClient.DeleteAsync($"{RequestUri}/{teamId}/projects/{projectId}");
			var response = await result.Content.ReadFromJsonAsync<ApiResponse>();
			response!.Success = result.IsSuccessStatusCode;
			return response!;
		}

		public async Task<ApiResponse> RemoveMemberFromTeamAsync(Guid memberId, Guid teamId) {
			var result = await httpClient.DeleteAsync($"{RequestUri}/{teamId}/members/{memberId}");
			var response = await result.Content.ReadFromJsonAsync<ApiResponse>();
			response!.Success = result.IsSuccessStatusCode;
			return response!;
		}
	}
}
