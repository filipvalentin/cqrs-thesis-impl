using Lunatic.Domain.Primitives;

namespace Lunatic.Domain.DomainEvents.Project {
	public record ProjectCreatedDomainEvent(Guid Id,
		Guid TeamId,
		Guid CreatedByUserId,
		string Title,
		string Description) : IDomainEvent;
}
