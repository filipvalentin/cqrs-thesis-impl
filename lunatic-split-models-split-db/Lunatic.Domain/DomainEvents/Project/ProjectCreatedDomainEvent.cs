using Lunatic.Domain.Primitives;

namespace Lunatic.Domain.DomainEvents.Project {
	public record ProjectCreatedDomainEvent(Guid Id) : IDomainEvent;
}
