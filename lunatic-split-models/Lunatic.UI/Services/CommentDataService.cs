using Lunatic.UI.Contracts;
using Lunatic.UI.Models.Dtos;
using Lunatic.UI.Services.Responses;
using System.Net.Http.Json;

namespace Lunatic.UI.Services {
	public class CommentDataService : ICommentDataService {
		private const string RequestUri = "api/v1/comments";
		private readonly HttpClient httpClient;
		private readonly ITokenService tokenService;

		public CommentDataService(HttpClient httpClient, ITokenService tokenService) {
			this.httpClient = httpClient;
			this.tokenService = tokenService;
		}

		public async Task<ApiResponse<CommentDto>> GetCommentByIdAsync(Guid commentId) {
			var result = await httpClient.GetAsync($"{RequestUri}/{commentId}");
			var response = await result.Content.ReadFromJsonAsync<ApiResponse<CommentDto>>();
			response!.Success = result.IsSuccessStatusCode;
			return response!;
		}

		public async Task<ApiResponse> UpdateCommentAsync(Guid commentId, string comment) {
			var result = await httpClient.PutAsJsonAsync($"{RequestUri}/{commentId}", new { CommentId = commentId, Content = comment });
			var response = await result.Content.ReadFromJsonAsync<ApiResponse>();
			response!.Success = result.IsSuccessStatusCode;
			return response!;
		}
	}
}
