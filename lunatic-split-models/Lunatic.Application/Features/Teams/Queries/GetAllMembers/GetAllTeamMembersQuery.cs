
using MediatR;


namespace Lunatic.Application.Features.Teams.Queries.GetAllMembers {
    public record GetAllTeamMembersQuery(Guid TeamId) : IRequest<GetAllTeamMembersQueryResponse>;
}
