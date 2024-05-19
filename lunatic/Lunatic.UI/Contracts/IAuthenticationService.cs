using Lunatic.UI.Models.Dtos;
using Lunatic.UI.Models.ViewModels;
using Lunatic.UI.Services.Responses;

namespace Lunatic.UI.Contracts {
	public interface IAuthenticationService {
		Task Login(LoginModel loginRequest);
		Task Register(RegisterModel registerRequest);
		Task Logout();
		Task<ApiResponse> ResetPassword(ResetPasswordDto resetPasswordDto);
		Task<ApiResponse> ConfirmPassword(ConfirmPasswordDto confirmPasswordDto);
	}
}
