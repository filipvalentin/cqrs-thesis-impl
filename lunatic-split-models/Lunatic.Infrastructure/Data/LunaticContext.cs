using Lunatic.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Task = Lunatic.Domain.Entities.Task;

namespace Lunatic.Infrastructure.Data {
	public sealed class LunaticContext : DbContext {
		public LunaticContext(DbContextOptions<LunaticContext> options) : base(options) { }

		public DbSet<Comment> Comments { get; set; }
		public DbSet<Task> Tasks { get; set; }
		public DbSet<Project> Projects { get; set; }
		public DbSet<Team> Teams { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<Image> Images { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			modelBuilder.ApplyConfigurationsFromAssembly(
				typeof(LunaticContext).Assembly,
				WriteConfigurationsFilter);
		}
		private static bool WriteConfigurationsFilter(Type type) =>
			 type.FullName?.Contains("Configurations.Write") ?? false;

	}
}

