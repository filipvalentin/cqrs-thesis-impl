using Lunatic.Domain.Primitives;

namespace Lunatic.Domain.DomainEvents {
	public record TaskCreatedDomainEvent(Guid TaskId) : IDomainEvent;
}
