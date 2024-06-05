using AutoMapper;
using Lunatic.Application.Features.Teams.Payload;
using Lunatic.Application.Persistence.ReadSide;
using MediatR;


namespace Lunatic.Application.Features.Teams.Queries.GetById {
	public class GetByIdTeamQueryHandler : IRequestHandler<GetByIdTeamQuery, GetByIdTeamQueryResponse> {
		private readonly ITeamReadSideRepository teamRepository;
		private readonly IMapper mapper;

		public GetByIdTeamQueryHandler(ITeamReadSideRepository teamRepository, IMapper mapper) {
			this.teamRepository = teamRepository;
			this.mapper = mapper;
		}

		public async Task<GetByIdTeamQueryResponse> Handle(GetByIdTeamQuery request, CancellationToken cancellationToken) {
            var result = await teamRepository.FindByIdAsync(request.TeamId);

            if(!result.IsSuccess) {
                return new GetByIdTeamQueryResponse {
					Success = false,
                    ValidationErrors = new List<string> { result.Error }
                };
            }

			return new GetByIdTeamQueryResponse {
                Success = true,
                Team = mapper.Map<TeamDto>(result.Value)
            };
        }
    }
}
