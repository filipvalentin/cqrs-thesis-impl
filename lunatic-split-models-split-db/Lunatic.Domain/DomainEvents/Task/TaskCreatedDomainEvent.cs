using Lunatic.Domain.Entities;
using Lunatic.Domain.Primitives;
using TaskStatus = Lunatic.Domain.Entities.TaskStatus;

namespace Lunatic.Domain.DomainEvents.Task {
	public record TaskCreatedDomainEvent(
		Guid Id,
		Guid ProjectId,
		Guid CreatedByUserId,
		string TaskSectionCard,
		string Title,
		string Description,
		TaskPriority Priority,
		TaskStatus Status,
		List<string> Tags,
		List<Guid> CommentIds,
		List<Guid> AssigneeIds,
		DateTime PlannedStartDate,
		DateTime PlannedEndDate,
		DateTime? StartedDate,
		DateTime? EndedDate) : IDomainEvent;
}
