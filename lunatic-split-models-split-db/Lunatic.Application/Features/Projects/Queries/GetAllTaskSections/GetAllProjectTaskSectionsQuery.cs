
using MediatR;


namespace Lunatic.Application.Features.Projects.Queries.GetAllTaskSections {
    public record GetAllProjectTaskSectionsQuery(Guid ProjectId) : IRequest<GetAllProjectTaskSectionsQueryResponse>;
}
