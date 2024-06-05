using Lunatic.Domain.Primitives;

namespace Lunatic.Domain.DomainEvents.User {
	public record UserDeletedDomainEvent(Guid Id) : IDomainEvent;
}
