using Lunatic.Domain.Primitives;

namespace Lunatic.Domain.DomainEvents.Task {
	public record TaskDeletedDomainEvent(Guid Id, List<Guid> CommentIds, bool Cascaded, Guid ProjectId) : IDomainEvent;
}
