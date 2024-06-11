using Lunatic.Domain.Primitives;

namespace Lunatic.Domain.DomainEvents.Team {
	public record TeamDisbandedDomainEvent(Guid Id, List<Guid> ProjectIds) : IDomainEvent;
}
