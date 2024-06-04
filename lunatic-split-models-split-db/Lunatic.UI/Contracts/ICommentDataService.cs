using Lunatic.UI.Models.Dtos;
using Lunatic.UI.Services.Responses;

namespace Lunatic.UI.Contracts {
	public interface ICommentDataService {

		Task<ApiResponse<CommentDto>> GetCommentByIdAsync(Guid commentId);

		Task<ApiResponse> UpdateCommentAsync(Guid commentId, string comment);
	}
}
