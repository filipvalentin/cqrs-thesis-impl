
using MediatR;


namespace Lunatic.Application.Features.Users.Queries.GetUsernameMatches {
	public class GetUsernameMatchesQuery : IRequest<GetUsernameMatchesQueryResponse> {
		public string UsernameMatch { get; set; } = default!;
	}
}
