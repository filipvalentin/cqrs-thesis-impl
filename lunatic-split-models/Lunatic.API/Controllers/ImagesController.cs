using Lunatic.Application.Contracts.Interfaces;
using Lunatic.Application.Models.Image;
using Lunatic.Application.Responses.Image;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Lunatic.API.Controllers {
	[ApiController]
	public class ImagesController : BaseApiController {
		private readonly IImageService imageService;
		private readonly IMemoryCache imageCache;

		public ImagesController(IImageService imageService, IMemoryCache imageCache) {
			this.imageService = imageService;
			this.imageCache = imageCache;
		}

		[HttpPost("{userId}")]
		[Produces("application/json")]
		public async Task<IActionResult> UploadUserImage(Guid userId, ImageUploadModel image) {

			var response = await imageService.UploadUserImage(userId, image);
			if (response.IsSuccess) {
				imageCache.Remove(userId);
				return Ok(response);
			}
			return BadRequest(response);
		}

		[HttpGet("{userId}")]
		[Produces("application/json")]
		public async Task<IActionResult> GetUserImage(Guid userId) {
			if (imageCache.TryGetValue(userId, out ImageResponse cachedResponse)) {
				return Ok(cachedResponse);
			}

			var cacheEntryOptions = new MemoryCacheEntryOptions()
				.SetSlidingExpiration(TimeSpan.FromSeconds(60))
				.SetAbsoluteExpiration(TimeSpan.FromSeconds(3600))
				.SetPriority(CacheItemPriority.Normal);

			var response = await imageService.GetUserImage(userId);

			if (response.Success) {
				imageCache.Set(userId, response, cacheEntryOptions);
			}

			return response.Success ? Ok(response) : BadRequest(response);
		}
	}
}
