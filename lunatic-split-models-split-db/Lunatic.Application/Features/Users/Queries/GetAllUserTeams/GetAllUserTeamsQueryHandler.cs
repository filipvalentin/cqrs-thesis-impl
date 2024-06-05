using Lunatic.Application.Features.Teams.Payload;
using MediatR;
using Lunatic.Application.Persistence.ReadSide;
using AutoMapper;


namespace Lunatic.Application.Features.Users.Queries.GetAllUserTeams {
	public class GetAllUserTeamsQueryHandler : IRequestHandler<GetAllUserTeamsQuery, GetAllUserTeamsQueryResponse> {
		private readonly IUserReadSideRepository userRepository;
		private readonly ITeamReadSideRepository teamRepository;
		private readonly IMapper mapper;

		public GetAllUserTeamsQueryHandler(IUserReadSideRepository userRepository, ITeamReadSideRepository teamRepository, IMapper mapper) {
			this.userRepository = userRepository;
			this.teamRepository = teamRepository;
			this.mapper = mapper;
		}

		public async Task<GetAllUserTeamsQueryResponse> Handle(GetAllUserTeamsQuery request, CancellationToken cancellationToken) {
			var userResult = await userRepository.FindByIdAsync(request.UserId);
			if (!userResult.IsSuccess) {
				return new GetAllUserTeamsQueryResponse {
					Success = false,
					ValidationErrors = new List<string> { "User not found" }
				};
			}

			var user = userResult.Value;

			GetAllUserTeamsQueryResponse response = new () { Success = true };

			var teams = user.TeamIds.Select(async teamId => await teamRepository.FindByIdAsync(teamId)).ToList();
			//foreach (var teamId in teamIds) {
			//	var team = (await teamRepository.FindByIdAsync(teamId)).Value;
			//	teams.Add(team);
			//}

			response.Teams = teams.Select(team => mapper.Map<TeamDto>(team)).ToList();
			return response;
		}
	}
}
