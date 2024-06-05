using Lunatic.Domain.Primitives;

namespace Lunatic.Domain.DomainEvents.Team {
	public record TeamCreatedDomainEvent(Guid Id) : IDomainEvent;
}
