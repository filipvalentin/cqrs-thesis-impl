using Lunatic.Application.Models.ReadModels.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Lunatic.Infrastructure.Data {
	public class LunaticReadContext : DbContext {
		public LunaticReadContext(DbContextOptions<LunaticReadContext> options) : base(options) { }

		public DbSet<CompositeTaskReadModel> CompoundTaskReadModel { get; set; }
		public DbSet<TaskDescriptionReadModel> TaskDescriptionReadModel { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			modelBuilder.ApplyConfigurationsFromAssembly(
				typeof(LunaticReadContext).Assembly,
				ReadConfigurationsFilter);
		}
		private static bool ReadConfigurationsFilter(Type type) =>
			 type.FullName?.Contains("Configurations.Read") ?? false;
	}
}

