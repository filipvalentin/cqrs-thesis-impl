namespace Lunatic.UI.Models.Dtos {
	public class UserProfileDto {
		public Guid UserId { get; set; } = default!;
		public string FirstName { get; set; } = default!;
		public string LastName { get; set; } = default!;
		public string Username { get; set; } = default!;
		public string Email { get; set; } = default!;
	}
}
