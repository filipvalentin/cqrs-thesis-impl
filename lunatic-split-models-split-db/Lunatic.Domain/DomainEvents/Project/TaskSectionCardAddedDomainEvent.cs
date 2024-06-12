using Lunatic.Domain.Primitives;

namespace Lunatic.Domain.DomainEvents.Project {
	public record TaskSectionCardAddedDomainEvent(Guid Id, string SectionCardName) : IDomainEvent;
}
