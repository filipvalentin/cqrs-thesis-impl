using Lunatic.Domain.Primitives;

namespace Lunatic.Domain.DomainEvents.Project {
	public record ProjectUpdatedDomainEvent(
		Guid Id,
		Guid TeamId,
		Guid CreatedByUserId,
		string Title,
		string Description,
		List<string> TaskSectionCards,
		List<Guid> TaskIds) : IDomainEvent;
}
