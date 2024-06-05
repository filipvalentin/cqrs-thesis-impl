using Lunatic.Domain.Primitives;

namespace Lunatic.Domain.DomainEvents.Task {
	public record TaskCreatedDomainEvent(Guid Id) : IDomainEvent;
}
