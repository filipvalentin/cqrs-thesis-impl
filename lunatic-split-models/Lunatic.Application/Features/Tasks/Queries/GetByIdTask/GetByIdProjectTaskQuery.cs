using MediatR;


namespace Lunatic.Application.Features.Tasks.Queries.GetByIdTask
{
    public class GetByIdProjectTaskQuery : IRequest<GetByIdProjectTaskQueryResponse>
    {
        public Guid TaskId { get; set; } = default!;
    }
}
