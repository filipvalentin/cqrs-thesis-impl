using MediatR;


namespace Lunatic.Application.Features.Tasks.Queries.GetByIdCompoundTask
{
    public class GetByIdCompoundTaskQuery : IRequest<GetByIdCompoundTaskQueryResponse>
    {
        public Guid TaskId { get; set; } = default!;
    }
}
