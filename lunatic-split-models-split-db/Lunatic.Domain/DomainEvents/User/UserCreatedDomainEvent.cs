using Lunatic.Domain.Primitives;

namespace Lunatic.Domain.DomainEvents.User {
	public record UserCreatedDomainEvent(Guid Id, Entities.User User) : IDomainEvent;
}
