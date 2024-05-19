using Lunatic.Application.Models.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lunatic.Infrastructure.Data.Configurations.Read {
	internal sealed class TaskConfiguration : IEntityTypeConfiguration<TaskReadModel> {
		public void Configure(EntityTypeBuilder<TaskReadModel> builder) {
			builder.ToTable("Tasks");
		}
	}
}
