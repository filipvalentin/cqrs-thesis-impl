using Lunatic.Application.Contracts.Identity;
using Lunatic.Application.Models.Identity;
using Lunatic.Identity.Models;
using Microsoft.AspNetCore.Mvc;


namespace Lunatic.API.Controllers {
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger) {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginModel model) {
            try {
                if(!ModelState.IsValid) {
                    return BadRequest("Invalid payload");
                }

                var response = await _authService.Login(model);

                if(!response.Success) {
                    return BadRequest(response);
                }

                return Ok(response);
            }
            catch (Exception ex) {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegistrationModel model) {
            try {
                if(!ModelState.IsValid) {
                    return BadRequest("Invalid payload");
                }

                var response = await _authService.Registeration(model, UserRoles.User);

                if(!response.Success) {
                    return BadRequest(response);
                }

                return Ok(response);
            }
            catch (Exception ex) {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("changepassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model) {
            try {
                if(!ModelState.IsValid) {
                    return BadRequest("Invalid payload");
                }

                var response = await _authService.ChangePassword(model);

                if(!response.Success) {
                    return BadRequest(response);
                }

                return Ok(response);
            }
            catch (Exception ex) {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("resetpassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model) {
            try {
                if(!ModelState.IsValid) {
                    return BadRequest("Invalid payload");
                }

                var response = await _authService.ResetPassword(model);

                if(!response.Success) {
                    return BadRequest(response);
                }

                return Ok(response);
            }
            catch (Exception ex) {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("confirmpassword")]
        public async Task<IActionResult> ConfirmPassword(ConfirmPasswordModel model) {
            try {
                if(!ModelState.IsValid) {
                    return BadRequest("Invalid payload");
                }

                var response = await _authService.ConfirmPassword(model);

                if(!response.Success) {
                    return BadRequest(response);
                }

                return Ok(response);
            }
            catch (Exception ex) {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

		[HttpPost]
		[Route("logout")]
		public async Task<IActionResult> Logout()
		{
			await _authService.Logout();
			return Ok();
		}
	}
}
