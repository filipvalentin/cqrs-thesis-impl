using Lunatic.Domain.Primitives;

namespace Lunatic.Domain.DomainEvents.Task {
	public record TaskCompletedDomainEvent(Guid TaskId) : IDomainEvent;
}
