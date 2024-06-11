using Lunatic.Domain.Primitives;

namespace Lunatic.Domain.DomainEvents.Comment {
	public record CommentAddedDomainEvent(Guid Id, 
		Guid TaskId,
		Guid CreatedByUserId,
		string Content,
		DateTime CreatedDate,
		DateTime LastModifiedDate
		) : IDomainEvent {
	}
}
