
using Lunatic.Application.Models.Identity;
using Lunatic.Application.Responses.Identity;


namespace Lunatic.Application.Contracts.Identity {
    public interface IAuthService {
        Task<RegisterResponse> Registration(RegistrationModel model, string role);
        Task<LoginResponse> Login(LoginModel model);
        Task<ChangePasswordResponse> ChangePassword(ChangePasswordModel model);
        Task<ResetPasswordResponse> ResetPassword(ResetPasswordModel model);
        Task<ConfirmPasswordResponse> ConfirmPassword(ConfirmPasswordModel model);
        Task<(int, string)> Logout();
	}
}
