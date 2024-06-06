using Lunatic.Domain.Primitives;

namespace Lunatic.Domain.DomainEvents.Project {
	public record ProjectDeletedDomainEvent(Guid Id, Entities.Project Project) : IDomainEvent;
}
