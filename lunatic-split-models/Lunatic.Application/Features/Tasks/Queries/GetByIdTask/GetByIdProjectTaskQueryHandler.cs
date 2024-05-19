
using Lunatic.Application.Persistence;
using Lunatic.Application.Features.Tasks.Payload;
using MediatR;


namespace Lunatic.Application.Features.Tasks.Queries.GetByIdTask {
	public class GetByIdProjectTaskQueryHandler : IRequestHandler<GetByIdProjectTaskQuery, GetByIdProjectTaskQueryResponse> {
		private readonly ITaskReadService taskReadService;

		public GetByIdProjectTaskQueryHandler(ITaskReadService taskReadService) {
			this.taskReadService = taskReadService;
		}


		public async Task<GetByIdProjectTaskQueryResponse> Handle(GetByIdProjectTaskQuery request, CancellationToken cancellationToken) {

			var result = await taskReadService.GetByIdAsync(request.TaskId);

			if (!result.IsSuccess) {
				return new GetByIdProjectTaskQueryResponse {
					Success = false,
					ValidationErrors = new List<string> { result.Error }
				};
			}
			var taskReadModel = result.Value;

			return new GetByIdProjectTaskQueryResponse {
				Success = true,
				Task = new TaskDto {
					CreatedByUserId = taskReadModel.CreatedByUserId,
					TaskId = taskReadModel.Id,
					ProjectId = taskReadModel.ProjectId,
					Section = taskReadModel.TaskSectionCard,
					Title = taskReadModel.Title,
					Description = taskReadModel.Description,
					Priority = taskReadModel.Priority,
					Status = taskReadModel.Status,
					Tags = taskReadModel.Tags,
					CommentIds = taskReadModel.CommentIds,
					AssigneeIds = taskReadModel.AssigneeIds,
					PlannedStartDate = taskReadModel.PlannedStartDate,
					PlannedEndDate = taskReadModel.PlannedEndDate,
					StartedDate = taskReadModel.StartedDate,
					EndedDate = taskReadModel.EndedDate,
				}
			};
		}
	}
}
