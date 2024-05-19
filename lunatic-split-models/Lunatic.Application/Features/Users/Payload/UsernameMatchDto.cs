namespace Lunatic.Application.Features.Users.Payload {
	public class UsernameMatchDto {
		public Guid UserId { get; set; }
		public string Username { get; set; } = default!;
	}
}
