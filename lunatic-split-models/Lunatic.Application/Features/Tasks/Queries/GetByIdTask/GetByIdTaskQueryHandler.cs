using Lunatic.Application.Features.Tasks.Interfaces;
using Lunatic.Application.Features.Tasks.Payload;
using MediatR;


namespace Lunatic.Application.Features.Tasks.Queries.GetByIdTask
{
    public class GetByIdTaskQueryHandler : IRequestHandler<GetByIdTaskQuery, GetByIdTaskQueryResponse> {
		private readonly ITaskReadService taskReadService;

		public GetByIdTaskQueryHandler(ITaskReadService taskReadService) {
			this.taskReadService = taskReadService;
		}


		public async Task<GetByIdTaskQueryResponse> Handle(GetByIdTaskQuery request, CancellationToken cancellationToken) {

			var result = await taskReadService.GetByIdAsync(request.TaskId);

			if (!result.IsSuccess) {
				return new GetByIdTaskQueryResponse {
					Success = false,
					ValidationErrors = new List<string> { result.Error }
				};
			}
			var taskReadModel = result.Value;

			return new GetByIdTaskQueryResponse {
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
