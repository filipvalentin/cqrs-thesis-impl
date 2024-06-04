using Lunatic.UI.Models.Dtos;
using Lunatic.UI.Models.ViewModels;
using Lunatic.UI.Services.Responses;

namespace Lunatic.UI.Contracts {
	public interface IUserDataService {
		Task<ApiResponse<UserDto>> GetUserByIdAsync(Guid id);
		Task<List<UsernameMatchDto>> GetUsersByUsernamePartialMatchAsync(string usernameMatch);
		Task<ApiResponse> UpdateUserInfoAsync(UpdateUserInfoViewModel user);
		Task<ApiResponse> ChangeUserPasswordAsync(Guid id, ChangeUserPasswordDto changeUserPasswordDto);
		Task<ApiResponse<List<TeamDto>>> GetUserTeamsAsync(Guid userId);
	}
}
