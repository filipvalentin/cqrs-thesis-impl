using Microsoft.AspNetCore.Http;

namespace Lunatic.Application.Models.Image;

public class ImageUploadModel {
	public IFormFile File { get; set; }
}
