
using MediatR;


namespace Lunatic.Application.Features.Users.Queries.GetUsernameMatches {
	public class GetUsernameMatchesQuery : IRequest<GetUsernameMatchesQueryResponse> {
		public string StartingWith { get; set; } = default!;
		public int Take { get; set; } = default!;
	}
}
