
using Lunatic.Domain.MLModel;
using Microsoft.EntityFrameworkCore;

namespace Lunatic.Infrastructure.Data {
	public sealed class LunaticMLContext : DbContext {
		public LunaticMLContext(DbContextOptions<LunaticMLContext> options) : base(options) { }
		public DbSet<DaysToCompleteTaskEntry> DaysToCompleteTaskEntries { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			modelBuilder.Entity<DaysToCompleteTaskEntry>(
				builder => {
					builder.HasKey(e => e.TaskId);
				});
		}
	}
}

