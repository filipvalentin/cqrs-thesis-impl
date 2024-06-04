using Lunatic.Application.Contracts.Interfaces;
using Lunatic.Application.Models.Image;
using Lunatic.Application.Persistence.WriteSide;
using Lunatic.Application.Responses.Image;
using Lunatic.Domain.Entities;
using Lunatic.Domain.Utils;

namespace Lunatic.Infrastructure.Services
{
    public class ImageService : IImageService {
		private readonly IImageRepository imageRepository;
		private readonly IUserRepository userRepository;
		public ImageService(IImageRepository imageRepository, IUserRepository userRepository) {
			this.imageRepository = imageRepository;
			this.userRepository = userRepository;
		}

		public async Task<ImageResponse> GetUserImage(Guid userId) {

			var user = await userRepository.FindByIdAsync(userId);
			if (!user.IsSuccess) {
				return new ImageResponse {
					Success = false,
					Message = "User not found"
				};
			}

			var found = await imageRepository.FindByUserIdAsync(userId);
			if (!found.IsSuccess) {
				return new ImageResponse {
					Success = false,
					Message = "User never uploaded an image",
				};
			}
			return new ImageResponse {
				Success = true,
				Image = new ImageDto { ImageData = found.Value.ImageData }
			};
		}

		public async Task<Result> UploadUserImage(Guid userId, ImageUploadModel image) {
			//if user exists
			var user = await userRepository.FindByIdAsync(userId);
			if (!user.IsSuccess) {
				return Result.Failure("User not found");
			}

			//get the image bytes from IFormFile
			byte[]? imageBytes = null;
			using (var memoryStream = new MemoryStream()) {
				await image.File.CopyToAsync(memoryStream);
				imageBytes = memoryStream.ToArray();
			}
			if (imageBytes == null) {
				return Result.Failure("Image bytes are null");
			}

			var found = await imageRepository.FindByUserIdAsync(userId);
			if (!found.IsSuccess) {
				var imageResult = Image.Create(userId, imageBytes);
				await imageRepository.AddAsync(imageResult.Value);
			}
			else {
				found.Value.Update(imageBytes);
				await imageRepository.UpdateAsync(found.Value);
			}

			return Result.Success();
		}
	}
}
