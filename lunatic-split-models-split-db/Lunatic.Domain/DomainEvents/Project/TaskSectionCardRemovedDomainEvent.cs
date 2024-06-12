using Lunatic.Domain.Primitives;

namespace Lunatic.Domain.DomainEvents.Project {
	public record TaskSectionCardRemovedDomainEvent(Guid Id, string SectionCardName) : IDomainEvent;
}
