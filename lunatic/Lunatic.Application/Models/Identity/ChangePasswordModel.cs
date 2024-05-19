
using System.ComponentModel.DataAnnotations;


namespace Lunatic.Application.Models.Identity {
    public class ChangePasswordModel {
        [Required(ErrorMessage = "UserId is required")]
        public string UserId { get; set; } = default!;

        [Required(ErrorMessage = "OldPassword is required")]
        public string OldPassword { get; set; } = default!;

        [Required(ErrorMessage = "NewPassword is required")]
        public string NewPassword { get; set; } = default!;
    }
}
