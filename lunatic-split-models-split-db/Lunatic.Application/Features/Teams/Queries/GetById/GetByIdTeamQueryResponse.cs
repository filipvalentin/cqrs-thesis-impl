
using Lunatic.Application.Responses;
using Lunatic.Application.Features.Teams.Payload;


namespace Lunatic.Application.Features.Teams.Queries.GetById {
    public class GetByIdTeamQueryResponse : BaseResponse {
        public GetByIdTeamQueryResponse() : base() {}

        public TeamDto Team { get; set; } = default!;
    }
}

