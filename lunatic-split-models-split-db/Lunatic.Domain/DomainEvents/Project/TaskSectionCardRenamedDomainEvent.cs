using Lunatic.Domain.Primitives;

namespace Lunatic.Domain.DomainEvents.Project {
	public record TaskSectionCardRenamedDomainEvent(Guid Id, string SectionCardName, string NewSectionCardName) : IDomainEvent;
}
