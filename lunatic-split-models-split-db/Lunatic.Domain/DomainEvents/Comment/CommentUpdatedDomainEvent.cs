using Lunatic.Domain.Primitives;

namespace Lunatic.Domain.DomainEvents.Comment {
	public record CommentUpdatedDomainEvent(Guid Id) : IDomainEvent;
}
