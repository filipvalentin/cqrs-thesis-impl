using Lunatic.UI.Models.Dtos;
using Lunatic.UI.Models.ViewModels;
using Lunatic.UI.Services.Responses;

namespace Lunatic.UI.Contracts {
	public interface ITeamDataService {
		Task<ApiResponse<TeamDto>> CreateTeamAsync(TeamViewModel team);
		Task<ApiResponse> EditTeamInfoAsync(Guid teamId, EditTeamInfoViewModel editTeamInfoViewModel);
		Task<ApiResponse<TeamDto>> UpdateTeamInfoAsync(UpdateTeamInfoViewModel viewModel);
		Task<ApiResponse> DeleteTeamAsync(Guid teamId);
		Task<ApiResponse<TeamDto>> GetTeamByIdAsync(Guid teamId);
		Task<ApiResponse<List<UserDto>>> GetTeamMembersAsync(Guid teamId);
		Task<ApiResponse> AddMemberToTeamAsync(Guid memberId, Guid teamId);
		Task<ApiResponse<ProjectDto>> AddNewTeamProjectAsync(Guid teamId, ProjectViewModel projectViewModel);
		Task<ApiResponse> DeleteProjectAsync(Guid teamId, Guid projectId);
		Task<ApiResponse> RemoveMemberFromTeamAsync(Guid memberId, Guid teamId);

	}
}
