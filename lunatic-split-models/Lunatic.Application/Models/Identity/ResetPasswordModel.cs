
using System.ComponentModel.DataAnnotations;


namespace Lunatic.Application.Models.Identity {
    public class ResetPasswordModel {
        [Required(ErrorMessage = "A valid email address is required")]
        [EmailAddress(ErrorMessage = "A valid email address is required")]
        public string Email { get; set; } = default!;
    }
}
