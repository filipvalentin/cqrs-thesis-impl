using Lunatic.UI.Models.Dtos;
using Lunatic.UI.Services.Responses;

namespace Lunatic.UI.Contracts {
	public interface IImageService {
		public Task<ApiResponse<ImageDto>> GetUserImageAsync(Guid userId);
		public Task<ApiResponse> UploadUserImageAsync(Guid userId, HttpContent uploadImageDto);
	}
}
