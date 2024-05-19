
using MediatR;


namespace Lunatic.Application.Features.Users.Queries.GetAllUserTeams {
    public record GetAllUserTeamsQuery(Guid UserId) : IRequest<GetAllUserTeamsQueryResponse>;
}
