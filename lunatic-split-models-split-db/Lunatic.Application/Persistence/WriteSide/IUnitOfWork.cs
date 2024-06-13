namespace Lunatic.Application.Persistence.WriteSide {
	public interface IUnitOfWork {
		Task SaveChangesAsync(CancellationToken cancellationToken = default);
	}
}
