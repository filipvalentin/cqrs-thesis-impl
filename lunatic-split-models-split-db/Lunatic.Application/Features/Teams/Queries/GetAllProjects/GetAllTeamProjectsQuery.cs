
using MediatR;


namespace Lunatic.Application.Features.Teams.Queries.GetAllProjects {
    public record GetAllTeamProjectsQuery(Guid TeamId) : IRequest<GetAllTeamProjectsQueryResponse>;
}
