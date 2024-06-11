using Lunatic.Domain.Primitives;

namespace Lunatic.Domain.DomainEvents.Team {
	public record TeamMemberRemovedDomainEvent(
		Guid Id,
		Guid MemberId
		) : IDomainEvent;
}
