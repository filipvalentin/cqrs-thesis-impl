using MediatR;

namespace Lunatic.Domain.Primitives {
	public interface IDomainEvent : INotification {
		public Guid Id { get; init; }
	}
}
