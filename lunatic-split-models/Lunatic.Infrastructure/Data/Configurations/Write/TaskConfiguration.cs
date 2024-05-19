using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lunatic.Infrastructure.Data.Configurations.Write {
	internal sealed class TaskConfiguration : IEntityTypeConfiguration<Domain.Entities.Task>{
		public void Configure(EntityTypeBuilder<Domain.Entities.Task> builder) {
			builder.ToTable("Tasks");
		}
	}
}
