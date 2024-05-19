
using System.Security.Claims;


namespace Lunatic.Application.Contracts.Interfaces {
    public interface ICurrentUserService {
        string UserId { get; }
        ClaimsPrincipal GetCurrentClaimsPrincipal();
        string GetCurrentUserId();
    }
}
