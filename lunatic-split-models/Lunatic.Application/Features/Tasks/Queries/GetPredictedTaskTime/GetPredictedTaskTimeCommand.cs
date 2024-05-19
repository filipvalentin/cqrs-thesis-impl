using MediatR;


namespace Lunatic.Application.Features.Tasks.Queries.GetPredictedTaskTime
{
    public class GetPredictedTaskTimeCommand : IRequest<GetPredictedTaskTimeCommandResponse>
    {
        public Guid TaskId { get; set; }
    }
}
