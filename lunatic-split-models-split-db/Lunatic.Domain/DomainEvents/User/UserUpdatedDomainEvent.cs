using Lunatic.Domain.Primitives;

namespace Lunatic.Domain.DomainEvents.User {
	public record UserUpdatedDomainEvent(Guid Id) : IDomainEvent;
}
