
using MediatR;


namespace Lunatic.Application.Features.Teams.Queries.GetById {
    public record GetByIdTeamQuery(Guid TeamId) : IRequest<GetByIdTeamQueryResponse>;
}
