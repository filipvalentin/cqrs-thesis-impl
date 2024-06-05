using Lunatic.Domain.Primitives;

namespace Lunatic.Domain.DomainEvents.Comment {
	public record CommentCreatedDomainEvent(Guid Id) : IDomainEvent {
	}
}
