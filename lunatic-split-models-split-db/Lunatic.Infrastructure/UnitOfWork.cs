using Lunatic.Application.Persistence.WriteSide;
using Lunatic.Infrastructure.Data;

namespace Lunatic.Infrastructure {
	internal sealed class UnitOfWork(LunaticContext context) : IUnitOfWork {
		private readonly LunaticContext _context = context;
		public Task SaveChangesAsync(CancellationToken cancellationToken = default) {
			return _context.SaveChangesAsync(cancellationToken);
		}
	}
}
