using Lunatic.Domain.Utils;
using System.ComponentModel.DataAnnotations;

namespace Lunatic.Domain.Entities {
	public class Image {
		private Image(Guid userId, byte[] imageData) {
			UserId = userId;
			ImageData = imageData;
		}

		[Key]
		public Guid UserId { get; private set; }
		public byte[] ImageData { get; private set; }

		public static Result<Image> Create(Guid userId, byte[] imageData) {
			return Result<Image>.Success(new Image(userId, imageData));
		}

		public void Update(byte[] imageData) {
			ImageData = imageData;
		}
	}
}
