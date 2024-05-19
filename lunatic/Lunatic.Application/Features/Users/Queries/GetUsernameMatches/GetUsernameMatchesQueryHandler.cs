
using Lunatic.Application.Features.Users.Payload;
using Lunatic.Application.Persistence;
using MediatR;


namespace Lunatic.Application.Features.Users.Queries.GetUsernameMatches {
	public class GetUsernameMatchesQueryHandler : IRequestHandler<GetUsernameMatchesQuery, GetUsernameMatchesQueryResponse> {
		private readonly IUserRepository userRepository;

		public GetUsernameMatchesQueryHandler(IUserRepository userRepository) {
			this.userRepository = userRepository;
		}

		public async Task<GetUsernameMatchesQueryResponse> Handle(GetUsernameMatchesQuery request, CancellationToken cancellationToken) {
			GetUsernameMatchesQueryResponse response = new GetUsernameMatchesQueryResponse();
			var users = await userRepository.GetAllAsync();

			if (users.IsSuccess) {
				var usersFiltered = from user in users.Value select user;

				if (!string.IsNullOrWhiteSpace(request.UsernameMatch)) {
					usersFiltered = from user in usersFiltered where user.Username.StartsWith(request.UsernameMatch) select user;
				}

				response.Matches = usersFiltered.Select(user => new UsernameMatchDto {
					UserId = user.Id,
					Username = user.Username,
				}).Take(10).ToList();
			}
			return response;
		}
	}
}
