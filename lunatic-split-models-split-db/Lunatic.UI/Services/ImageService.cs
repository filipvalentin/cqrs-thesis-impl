using Lunatic.UI.Contracts;
using Lunatic.UI.Models.Dtos;
using Lunatic.UI.Services.Responses;
using System.Net.Http.Json;

namespace Lunatic.UI.Services {
	public class ImageService : IImageService {
		private readonly HttpClient httpClient;
		//readonly ITokenService tokenService;
		private const string RequestUri = "api/v1/images";

		public ImageService(HttpClient httpClient) {
			this.httpClient = httpClient;
		}

		public async Task<ApiResponse<ImageDto>> GetUserImageAsync(Guid userId) {
			var result = await httpClient.GetAsync($"{RequestUri}/{userId}");
			var response = await result.Content.ReadFromJsonAsync<ApiResponse<ImageDto>>();
			response!.Success = result.IsSuccessStatusCode;
			return response!;
		}

		public async Task<ApiResponse> UploadUserImageAsync(Guid userId, HttpContent uploadImageDto) {
			var result = await httpClient.PostAsync($"{RequestUri}/{userId}", uploadImageDto);
			var response = await result.Content.ReadFromJsonAsync<ApiResponse>();
			response!.Success = result.IsSuccessStatusCode;
			return response!;
		}
	}
}
