namespace Lunatic.UI.Models.ViewModels {
	public class UpdateUserInfoViewModel {//dto
		public Guid UserId { get; set; } = default!;
		public string FirstName { get; set; } = default!;
		public string LastName { get; set; } = default!;
		public string Email { get; set; } = default!;
	}
}
