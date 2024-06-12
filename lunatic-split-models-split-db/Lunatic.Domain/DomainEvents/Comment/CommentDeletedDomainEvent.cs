using Lunatic.Domain.Primitives;

namespace Lunatic.Domain.DomainEvents.Comment {
	public record CommentDeletedDomainEvent(Guid Id) : IDomainEvent;
}
