using Lunatic.Domain.Primitives;

namespace Lunatic.Domain.DomainEvents.Team {
	public record TeamUpdatedDomainEvent(Guid Id,
		Guid CreatedByUserId,
		string Name,
		string Description,
		List<Guid> MemberIds,
		List<Guid> ProjectIds) : IDomainEvent {
	}
}
