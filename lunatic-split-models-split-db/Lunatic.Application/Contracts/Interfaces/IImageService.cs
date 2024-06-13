using Lunatic.Application.Models.Image;
using Lunatic.Application.Responses.Image;
using Lunatic.Domain.Utils;

namespace Lunatic.Application.Contracts.Interfaces {
	public interface IImageService {
		Task<Result> UploadUserImage(Guid userId, ImageUploadModel image);
		Task<ImageResponse> GetUserImage(Guid userId);
		Task<Result> DeleteUserImage(Guid userId);
	}
}
