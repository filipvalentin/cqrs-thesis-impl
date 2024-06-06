using Lunatic.Application.Features.Users.Payload;
using MediatR;
using Lunatic.Application.Persistence.ReadSide;
using AutoMapper;


namespace Lunatic.Application.Features.Teams.Queries.GetAllMembers {
	public class GetAllTeamMembersQueryHandler : IRequestHandler<GetAllTeamMembersQuery, GetAllTeamMembersQueryResponse> {
		private readonly ITeamReadSideRepository teamRepository;
		private readonly IUserReadSideRepository userRepository;
		private readonly IMapper mapper;

		public GetAllTeamMembersQueryHandler(ITeamReadSideRepository teamRepository, IUserReadSideRepository userRepository, IMapper mapper) {
			this.teamRepository = teamRepository;
			this.userRepository = userRepository;
			this.mapper = mapper;
		}

		public async Task<GetAllTeamMembersQueryResponse> Handle(GetAllTeamMembersQuery request, CancellationToken cancellationToken) {
			var result = await teamRepository.GetTeamMembers(request.TeamId);

			if(!result.IsSuccess) {
				return new GetAllTeamMembersQueryResponse {
					Success = false,
					ValidationErrors = new List<string> { result.Error }
				};
			}

			return new GetAllTeamMembersQueryResponse {
				Members = result.Value.Select(member => mapper.Map<UserDto>(member)).ToList()
			};
		}
	}
}
