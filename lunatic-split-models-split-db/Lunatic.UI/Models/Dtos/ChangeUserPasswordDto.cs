namespace Lunatic.UI.Models.Dtos {
	public class ChangeUserPasswordDto {
		public Guid UserId { get; set; } = default!;
		public string OldPassword { get; set; } = default!;
		public string NewPassword { get; set; } = default!;
		public string ConfirmNewPassword { get; set; } = default!;
	}
}
