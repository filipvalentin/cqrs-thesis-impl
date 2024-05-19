using Blazored.LocalStorage;
using Lunatic.UI.Contracts;
using Lunatic.UI.Models.Dtos;
using Lunatic.UI.Models.ViewModels;
using Lunatic.UI.Services.Responses;
using System.Net.Http.Json;
using System.Text.Json.Nodes;

namespace Lunatic.UI.Services {
	public class AuthenticationService : IAuthenticationService {
		private const string RequestUri = "api/v1/auth";
		private readonly HttpClient httpClient;
		private readonly ITokenService tokenService;
		private readonly ILocalStorageService localStorageService;

		public AuthenticationService(HttpClient httpClient, ITokenService tokenService, ILocalStorageService localStorageService) {
			this.httpClient = httpClient;
			this.tokenService = tokenService;
			this.localStorageService = localStorageService;
		}

		public async Task Login(LoginModel loginRequest) {
			var response = await httpClient.PostAsJsonAsync($"{RequestUri}/login", loginRequest);
			if (response.StatusCode == System.Net.HttpStatusCode.BadRequest) {
				throw new Exception(await response.Content.ReadAsStringAsync());
			}
			response.EnsureSuccessStatusCode();
			var json = await response.Content.ReadAsStringAsync();
			var result = JsonNode.Parse(json);
			var token = result!["token"];
			var userId = result!["userId"];

			await tokenService.SetTokenAsync(token.ToString());
			await localStorageService.SetItemAsync("userId", userId);
		}

		public async Task Logout() {
			await tokenService.RemoveTokenAsync();
			var result = await httpClient.PostAsync($"{RequestUri}/logout", null);
			result.EnsureSuccessStatusCode();
		}

		public async Task Register(RegisterModel registerRequest) {
			var result = await httpClient.PostAsJsonAsync($"{RequestUri}/register", registerRequest);
			if (result.StatusCode == System.Net.HttpStatusCode.BadRequest) {
				throw new Exception(await result.Content.ReadAsStringAsync());
			}
			result.EnsureSuccessStatusCode();
		}

		public async Task<ApiResponse> ResetPassword(ResetPasswordDto resetPasswordDto) {
			var result = await httpClient.PostAsJsonAsync($"{RequestUri}/resetpassword", resetPasswordDto);
			var response = await result.Content.ReadFromJsonAsync<ApiResponse>();
			response!.Success = result.IsSuccessStatusCode;
			return response!;
		}

		public async Task<ApiResponse> ConfirmPassword(ConfirmPasswordDto confirmPasswordDto) {
			var result = await httpClient.PostAsJsonAsync($"{RequestUri}/confirmpassword", confirmPasswordDto);
			var response = await result.Content.ReadFromJsonAsync<ApiResponse>();
			response!.Success = result.IsSuccessStatusCode;
			return response!;
		}

	}
}
