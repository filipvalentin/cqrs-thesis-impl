
using System.ComponentModel.DataAnnotations;


namespace Lunatic.Application.Models.Identity {
    public class ConfirmPasswordModel {
        [Required(ErrorMessage = "Token is required")]
        public string Token { get; set; } = default!;

        [Required(ErrorMessage = "NewPassword is required")]
        public string NewPassword { get; set; } = default!;
    }
}
