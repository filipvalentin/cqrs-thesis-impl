using Lunatic.Domain.Primitives;

namespace Lunatic.Domain.DomainEvents {
	public record TaskCompletedDomainEvent(Guid TaskId) : IDomainEvent;
}
