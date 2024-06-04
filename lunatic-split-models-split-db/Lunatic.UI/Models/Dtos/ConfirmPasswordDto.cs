namespace Lunatic.UI.Models.Dtos {
	public class ConfirmPasswordDto {
		public string Token { get; set; } = default!;
		public string NewPassword { get; set; } = default!;
	}
}
