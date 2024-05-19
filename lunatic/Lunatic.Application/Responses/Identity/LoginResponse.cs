

namespace Lunatic.Application.Responses.Identity {
    public class LoginResponse : BaseResponse {
        public LoginResponse() : base() { }

        public string Token { get; set; } = default!;
        public string UserId { get; set; } = default!;
    }
}
