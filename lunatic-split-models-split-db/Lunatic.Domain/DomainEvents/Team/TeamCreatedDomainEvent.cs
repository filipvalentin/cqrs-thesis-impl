﻿using Lunatic.Domain.Primitives;

namespace Lunatic.Domain.DomainEvents.Team {
	public record TeamCreatedDomainEvent(
		Guid Id,
		Guid CreatedByUserId,
		string Name,
		string Description
		) : IDomainEvent;
}
