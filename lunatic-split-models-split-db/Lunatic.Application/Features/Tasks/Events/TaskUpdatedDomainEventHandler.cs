using AutoMapper;
using Lunatic.Application.Models.ReadModels;
using Lunatic.Application.Models.ReadModels.Tasks;
using Lunatic.Application.Persistence.ReadSide.Task;
using Lunatic.Application.Persistence.WriteSide;
using Lunatic.Application.Utils.Services;
using Lunatic.Domain.DomainEvents.Task;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Lunatic.Application.Features.Tasks.Events {
	public class TaskUpdatedDomainEventHandler(
		ITaskReadSideRepository taskReadRepository,
		IMapper mapper,
		IEventQueueService queueService,
		ILogger<TaskUpdatedDomainEventHandler> logger,
		IFlatTaskReadSideRepository flatTaskReadRepository,
		ICommentRepository commentRepository) : INotificationHandler<TaskUpdatedDomainEvent> {

		private readonly ITaskReadSideRepository taskReadRepository = taskReadRepository;
		private readonly IFlatTaskReadSideRepository flatTaskReadRepository = flatTaskReadRepository;
		private readonly ICommentRepository commentRepository = commentRepository;
		private readonly IMapper mapper = mapper;
		private readonly ILogger<TaskUpdatedDomainEventHandler> logger = logger;
		private readonly IEventQueueService queueService = queueService;

		public async Task Handle(TaskUpdatedDomainEvent notification, CancellationToken cancellationToken) {
			var updateResult = await taskReadRepository.UpdateAsync(notification.Id, mapper.Map<TaskReadModel>(notification));
			if (!updateResult.IsSuccess) {
				logger.LogError("Error from TaskReadSideRepository: {Error} when updating entity with {Id}", updateResult.Error, notification.Id);
				queueService.Enqueue(notification);
			}

			var commentList = new List<CommentReadModel>();
			foreach (var comment in notification.CommentIds) {
				var commentResult = await commentRepository.FindByIdAsync(comment);
				if (!commentResult.IsSuccess) {
					logger.LogError("Error from CommentReadSideRepository: {Error} when finding entity with {Id}", commentResult.Error, comment);
					queueService.Enqueue(notification);
				}
				commentList.Add(new CommentReadModel {
					Id = commentResult.Value.CommentId,
					TaskId = commentResult.Value.TaskId,
					CreatedByUserId = commentResult.Value.CreatedByUserId,
					Content = commentResult.Value.Content,
					CreatedDate = commentResult.Value.CreatedDate,
					LastModifiedDate = commentResult.Value.LastModifiedDate				
				});
			}
			var flatUpdateResult = await flatTaskReadRepository.UpdateAsync(notification.Id,
				new FlatTaskReadModel {
					Id = notification.Id,
					AssigneeIds = notification.AssigneeIds,
					Comments = commentList,
					CreatedByUserId = notification.CreatedByUserId,
					Description = notification.Description,
					EndedDate = notification.EndedDate,
					PlannedEndDate = notification.PlannedEndDate,
					PlannedStartDate = notification.PlannedStartDate,
					Priority = notification.Priority,
					ProjectId = notification.ProjectId,
					StartedDate = notification.StartedDate,
					Status = notification.Status,
					Tags = notification.Tags,
					TaskSectionCard = notification.TaskSectionCard,
					Title = notification.Title
				});
			if (!flatUpdateResult.IsSuccess) {
				logger.LogError("Error from FlatTaskReadSideRepository: {Error} when updating entity with {Id}", flatUpdateResult.Error, notification.Id);
				queueService.Enqueue(notification);
			}
		}
	}
}
