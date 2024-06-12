using Lunatic.Domain.Primitives;

namespace Lunatic.Domain.DomainEvents.Comment {
	public record CommentDeletedDomainEvent(Guid Id, bool Cascaded, Guid TaskId) : IDomainEvent;
}
