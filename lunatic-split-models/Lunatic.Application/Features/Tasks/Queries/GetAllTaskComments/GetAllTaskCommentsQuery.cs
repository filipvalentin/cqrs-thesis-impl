using MediatR;


namespace Lunatic.Application.Features.Tasks.Queries.GetAllTaskComments {
	public record GetAllTaskCommentsQuery(Guid TaskId) : IRequest<GetAllTaskCommentsQueryResponse>;
}
