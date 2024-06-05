using Lunatic.Application.Features.Users.Payload;
using Lunatic.Application.Persistence.ReadSide;
using MediatR;


namespace Lunatic.Application.Features.Users.Queries.GetUsernameMatches {
	public class GetUsernameMatchesQueryHandler : IRequestHandler<GetUsernameMatchesQuery, GetUsernameMatchesQueryResponse> {
		private readonly IUserReadSideRepository userRepository;

		public GetUsernameMatchesQueryHandler(IUserReadSideRepository userRepository) {
			this.userRepository = userRepository;
		}

		public async Task<GetUsernameMatchesQueryResponse> Handle(GetUsernameMatchesQuery request, CancellationToken cancellationToken) {
			var usernameMatchList = await userRepository.GetUsernameMatches(request.StartingWith, request.Take);

			return new GetUsernameMatchesQueryResponse {
				Matches = usernameMatchList.Select(usernameMatchList => new UsernameMatchDto {
					Username = usernameMatchList.Username,
					UserId = usernameMatchList.UserId
				}).ToList()
			};
		}
	}
}
