using System.ComponentModel.DataAnnotations;

namespace Lunatic.UI.Models.ViewModels {
	public class RegisterModel {
		[Required(ErrorMessage = "First Name is required")]
		public string FirstName { get; set; }

		[Required(ErrorMessage = "Last Name is required")]
		public string LastName { get; set; }

		[Required(ErrorMessage = "Username is required")]
		public string Username { get; set; }

		[Required(ErrorMessage = "A valid email address is required")]
		[EmailAddress(ErrorMessage = "A valid email address is required")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Password is required")]
		public string Password { get; set; }

		//[Required(ErrorMessage = "Confirm your password")]
		//[Compare(nameof(Password), ErrorMessage = "Passwords do not match")]
		//public string ConfirmPassword { get; set; }
	}
}
