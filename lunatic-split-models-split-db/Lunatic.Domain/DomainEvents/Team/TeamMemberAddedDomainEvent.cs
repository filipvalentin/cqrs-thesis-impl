using Lunatic.Domain.Primitives;

namespace Lunatic.Domain.DomainEvents.Team {
	public record TeamMemberAddedDomainEvent(
		Guid Id,
		Guid MemberId
		) : IDomainEvent;
}
