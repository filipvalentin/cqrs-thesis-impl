using Lunatic.Domain.Primitives;

namespace Lunatic.Domain.DomainEvents.Comment {
	//public record CommentAddedDomainEvent(
	//	Guid Id = default,
	//	Guid TaskId = default,
	//	Guid CreatedByUserId = default,
	//	string? Content = default,
	//	DateTime CreatedDate = default,
	//	DateTime LastModifiedDate = default
	//) : IDomainEvent;

	public record CommentAddedDomainEvent(
	Guid Id,
	Guid TaskId,
	Guid CreatedByUserId,
	string Content,
	DateTime CreatedDate,
	DateTime LastModifiedDate) : IDomainEvent;
}
