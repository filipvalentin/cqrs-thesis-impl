using Lunatic.UI.Contracts;
using Lunatic.UI.Models.Dtos;
using Lunatic.UI.Models.ViewModels;
using Lunatic.UI.Services.Responses;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Lunatic.UI.Services {
	public class UserDataService : IUserDataService {
		private const string RequestUri = "api/v1/users";
		private readonly HttpClient httpClient;
		private readonly ITokenService tokenService;

		public UserDataService(HttpClient httpClient, ITokenService tokenService) {
			this.httpClient = httpClient;
			this.tokenService = tokenService;
		}

		public async Task<ApiResponse> ChangeUserPasswordAsync(Guid id, ChangeUserPasswordDto changeUserPasswordDto) {
			var result = await httpClient.PutAsJsonAsync($"{RequestUri}/{id}/password", changeUserPasswordDto); //TODO endpoint
			var response = await result.Content.ReadFromJsonAsync<ApiResponse>();
			response!.Success = result.IsSuccessStatusCode;
			return response!;
		}

		public async Task<ApiResponse<UserDto>> GetUserByIdAsync(Guid userId) {
			httpClient.DefaultRequestHeaders.Authorization =
				new AuthenticationHeaderValue("bearer", await tokenService.GetTokenAsync());
			var result = await httpClient.GetAsync($"{RequestUri}/{userId}", HttpCompletionOption.ResponseHeadersRead);
			var response = await result.Content.ReadFromJsonAsync<ApiResponse<UserDto>>();
			response!.Success = result.IsSuccessStatusCode;
			return response!;
		}

		public async Task<List<UsernameMatchDto>> GetUsersByUsernamePartialMatchAsync(string usernameMatch) {
			httpClient.DefaultRequestHeaders.Authorization =
				new AuthenticationHeaderValue("bearer", await tokenService.GetTokenAsync());
			var result = await httpClient.GetAsync($"{RequestUri}/usernames/{usernameMatch}", HttpCompletionOption.ResponseHeadersRead);
			var response = await result.Content.ReadFromJsonAsync<List<UsernameMatchDto>>();
			return response!;
		}


		public async Task<ApiResponse> UpdateUserInfoAsync(UpdateUserInfoViewModel userViewModel) {
			httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", await tokenService.GetTokenAsync());
			var result = await httpClient.PutAsJsonAsync($"{RequestUri}/{userViewModel.UserId}", userViewModel);
			var response = await result.Content.ReadFromJsonAsync<ApiResponse>();
			response!.Success = result.IsSuccessStatusCode;
			return response!;
		}

		public async Task<ApiResponse<List<TeamDto>>> GetUserTeamsAsync(Guid userId) {
			var result = await httpClient.GetAsync($"{RequestUri}/{userId}/teams", HttpCompletionOption.ResponseHeadersRead);
			//result.EnsureSuccessStatusCode();
			var teams = await result.Content.ReadFromJsonAsync<ApiResponse<List<TeamDto>>>();
			teams!.Success = result.IsSuccessStatusCode;
			return teams!;
		}
	}
}
