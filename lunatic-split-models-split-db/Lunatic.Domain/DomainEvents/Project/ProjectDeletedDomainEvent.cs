using Lunatic.Domain.Primitives;

namespace Lunatic.Domain.DomainEvents.Project {
	public record ProjectDeletedDomainEvent(Guid Id, List<Guid> CommentIds) : IDomainEvent;
}
