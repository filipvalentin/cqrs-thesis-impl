using Lunatic.Application.Contracts.Identity;
using Lunatic.Application.Contracts;
using Lunatic.Application.Models.Identity;
using Lunatic.Application.Models;
using Lunatic.Domain.Entities;
using Lunatic.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Lunatic.Application.Responses.Identity;
using Microsoft.Extensions.Logging;
using Lunatic.Application.Persistence.WriteSide;


namespace Lunatic.Identity.Services
{
    public class AuthService : IAuthService {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IConfiguration configuration;
        private readonly IUserRepository userRepository;
        private readonly IEmailService emailService;
        private readonly ILogger<AuthService> logger;

        public AuthService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, IUserRepository userRepository, SignInManager<ApplicationUser> signInManager, IEmailService emailService, ILogger<AuthService> logger) {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.configuration = configuration;
            this.userRepository = userRepository;
            this.signInManager = signInManager;
            this.emailService = emailService;
            this.logger = logger;
        }

        public async Task<RegisterResponse> Registeration(RegistrationModel model, string role) {
            var userExists = await userManager.FindByNameAsync(model.Username);
            if(userExists != null) {
                return new RegisterResponse {
                    Success = false,
                    ValidationErrors = new List<string> { "User already exists" }
                };
            }

            var userDb = User.Create(model.FirstName, model.LastName, model.Email, model.Username, model.Password, Role.USER);
            if(!userDb.IsSuccess) {
                return new RegisterResponse {
                    Success = false,
                    ValidationErrors = new List<string> { "Can't create a user" }
                };
            }

            ApplicationUser user = new ApplicationUser() {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                Id = userDb.Value.Id.ToString(),
                UserName = model.Username,
            };

            var createUserResult = await userManager.CreateAsync(user, model.Password);
            if(!createUserResult.Succeeded) {
                return new RegisterResponse {
                    Success = false,
                    ValidationErrors = new List<string> { "User creation failed! Please check user details and try again." }
                };
            }

            if(!await roleManager.RoleExistsAsync(role)) {
                await roleManager.CreateAsync(new IdentityRole(role));
            }

            if(await roleManager.RoleExistsAsync(UserRoles.User)) {
                await userManager.AddToRoleAsync(user, role);
            }

            await userRepository.AddAsync(userDb.Value);

            return new RegisterResponse {
                Success = true,
            };
        }

        public async Task<LoginResponse> Login(LoginModel model) {
            var user = await userManager.FindByNameAsync(model.Username);
            if(user == null) {
                return new LoginResponse {
                    Success = false,
                    ValidationErrors = new List<string> { "Invalid username" }
                };
            }
            if(!await userManager.CheckPasswordAsync(user, model.Password)) {
                return new LoginResponse {
                    Success = false,
                    ValidationErrors = new List<string> { "Invalid password" }
                };
            }

            var userRoles = await userManager.GetRolesAsync(user);
            var authClaims = new List<Claim> {
               new Claim(ClaimTypes.Name, user.UserName!),
               new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            foreach (var userRole in userRoles) {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }
            string token = GenerateToken(authClaims);
            return new LoginResponse {
                Success = true,
                Token = token,
                UserId = user.Id
            };
        }

        public async Task<ChangePasswordResponse> ChangePassword(ChangePasswordModel model) {
            var user = await userManager.FindByIdAsync(model.UserId);
            if(user == null) {
                return new ChangePasswordResponse {
                    Success = false,
                    ValidationErrors = new List<string> { "Invalid UserId" }
                };
            }

            if(!await userManager.CheckPasswordAsync(user, model.OldPassword)) {
                return new ChangePasswordResponse {
                    Success = false,
                    ValidationErrors = new List<string> { "Invalid OldPassword" }
                };
            }


            var removePasswordResult = await userManager.RemovePasswordAsync(user);
            if(!removePasswordResult.Succeeded) {
                return new ChangePasswordResponse {
                    Success = false,
                    ValidationErrors = new List<string> { "Change Password failed! Please check user details and try again. Remove" }
                };
            }
            var addPasswordResult = await userManager.AddPasswordAsync(user, model.NewPassword);
            if(!addPasswordResult.Succeeded) {
                return new ChangePasswordResponse {
                    Success = false,
                    ValidationErrors = new List<string> { "Change Password failed! Please check user details and try again. Add" }
                };
            }

            return new ChangePasswordResponse {
                Success = true
            };
        }

        public async Task<ResetPasswordResponse> ResetPassword(ResetPasswordModel model) {
            var user = await userManager.FindByEmailAsync(model.Email);
            if(user == null) {
                return new ResetPasswordResponse {
                    Success = false,
                    ValidationErrors = new List<string> { "Invalid Email" }
                };
            }

            var token = await userManager.GeneratePasswordResetTokenAsync(user);

            var mail = new Mail {
                To = model.Email,
                Subject = "Reset Password",
                Body = $"Hacked by NASA here is the token: ?token={token}_{user.Id}"
            };

            try {
                await emailService.SendEmailAsync(mail);
            } catch(Exception exception) {
                logger.LogError(exception, "Email sending failed.");
                return new ResetPasswordResponse {
                    Success = false,
                    ValidationErrors = new List<string> { "Email sending failed" }
                };
            }

            return new ResetPasswordResponse {
                Success = true
            };
        }

        public async Task<ConfirmPasswordResponse> ConfirmPassword(ConfirmPasswordModel model) {
            if(!model.Token.Contains("_")) {
                return new ConfirmPasswordResponse {
                    Success = false,
                    ValidationErrors = new List<string> { "Invalid Token" }
                };
            }

            string[] split = model.Token.Split('_', 2);
            string token = split[0];
            string userId = split[1];
            var user = await userManager.FindByIdAsync(userId);
            if(user == null) {
                return new ConfirmPasswordResponse {
                    Success = false,
                    ValidationErrors = new List<string> { "Invalid Token" }
                };
            }

            var resetPasswordResult = await userManager.ResetPasswordAsync(user, token, model.NewPassword);
            if(!resetPasswordResult.Succeeded) {
                return new ConfirmPasswordResponse {
                    Success = false,
                    ValidationErrors = new List<string> { "Invalid Token" }
                };
            }


            return new ConfirmPasswordResponse {
                Success = true
            };
        }

		public async Task<(int, string)> Logout()
		{
			await signInManager.SignOutAsync();
			return (1, "User logged out successfully!");
		}

		private string GenerateToken(IEnumerable<Claim> claims) {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]!));

            var tokenDescriptor = new SecurityTokenDescriptor {
                Issuer = configuration["JWT:ValidIssuer"]!,
                Audience = configuration["JWT:ValidAudience"]!,
                Expires = DateTime.UtcNow.AddHours(3),
                SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(claims)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
