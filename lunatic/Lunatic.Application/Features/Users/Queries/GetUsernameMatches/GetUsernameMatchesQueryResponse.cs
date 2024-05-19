
using Lunatic.Application.Features.Users.Payload;


namespace Lunatic.Application.Features.Users.Queries.GetUsernameMatches {
	public class GetUsernameMatchesQueryResponse {
		public List<UsernameMatchDto> Matches { get; set; } = default!;
	}
}
