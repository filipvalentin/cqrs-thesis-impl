
using MediatR;


namespace Lunatic.Application.Features.Users.Queries.GetById {
    public record GetByIdUserQuery(Guid UserId) : IRequest<GetByIdUserQueryResponse>;
}

