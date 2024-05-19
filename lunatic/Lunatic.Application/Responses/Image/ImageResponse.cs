namespace Lunatic.Application.Responses.Image;

public class ImageResponse : BaseResponse {
	public ImageResponse() : base() { }
	public ImageDto Image { get; set; }
}
