
using System.ComponentModel.DataAnnotations;


namespace Lunatic.Application.Models.Identity {
    public class RegistrationModel {
        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; set; } = default!;

        [Required(ErrorMessage = "Last Name is required")]
        public string LastName { get; set; } = default!;

        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; } = default!;

        [Required(ErrorMessage = "A valid email address is required")]
        [EmailAddress(ErrorMessage = "A valid email address is required")]
        public string Email { get; set; } = default!;

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = default!;
    }
}
