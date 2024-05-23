using Lunatic.Application.Features.Tasks.Payload;
using MediatR;


namespace Lunatic.Application.Features.Tasks.Queries.GetByIdTaskDescription {
	public class GetByIdTaskDescriptionQueryHandler : IRequestHandler<GetByIdTaskDescriptionQuery, GetByIdTaskDescriptionQueryResponse> {
		private readonly ITaskReadService taskReadService;

		public GetByIdTaskDescriptionQueryHandler(ITaskReadService taskReadService) {
			this.taskReadService = taskReadService;
		}


		public async Task<GetByIdTaskDescriptionQueryResponse> Handle(GetByIdTaskDescriptionQuery request, CancellationToken cancellationToken) {

			var result = await taskReadService.GetTaskDescriptionByIdAsync(request.TaskId);

			if (!result.IsSuccess) {
				return new GetByIdTaskDescriptionQueryResponse {
					Success = false,
					ValidationErrors = new List<string> { result.Error }
				};
			}
			var taskDescriptionReadModel = result.Value;

			return new GetByIdTaskDescriptionQueryResponse {
				Success = true,
				Task = new TaskDescriptionDto {
					TaskId = taskDescriptionReadModel.Id,
					Section = taskDescriptionReadModel.TaskSectionCard,
					Title = taskDescriptionReadModel.Title,
					Description = taskDescriptionReadModel.Description,
					Priority = taskDescriptionReadModel.Priority,
					Tags = taskDescriptionReadModel.Tags
				}
			};
		}
	}
}
