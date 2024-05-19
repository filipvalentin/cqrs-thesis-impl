
using Lunatic.Application.Models.ReadModels;
using Microsoft.EntityFrameworkCore;

namespace Lunatic.Infrastructure.Data {
	public class LunaticReadContext : DbContext {
        public LunaticReadContext(DbContextOptions<LunaticReadContext> options) : base(options) {}

		public DbSet<CommentReadModel> Comments { get; set; }
		public DbSet<TaskReadModel> Tasks { get; set; }
		public DbSet<ProjectReadModel> Projects { get; set; }
		public DbSet<TeamReadModel> Teams { get; set; }
		//public DbSet<User> Users { get; set; }
		//public DbSet<Image> Images { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			modelBuilder.ApplyConfigurationsFromAssembly(
				typeof(LunaticReadContext).Assembly,
				ReadConfigurationsFilter);
		}
		private static bool ReadConfigurationsFilter(Type type) =>
			 type.FullName?.Contains("Configurations.Read") ?? false;
	}
}

