using Lunatic.Domain.Entities;
using Lunatic.Domain.Primitives;

namespace Lunatic.Domain.DomainEvents.User {
	public record UserCreatedDomainEvent(
		Guid Id,
		DateTime RegisteredAt,
		string FirstName,
		string LastName,
		string Email,
		string Username,
		Role Role,
		List<Guid> TeamIds) : IDomainEvent;
}
