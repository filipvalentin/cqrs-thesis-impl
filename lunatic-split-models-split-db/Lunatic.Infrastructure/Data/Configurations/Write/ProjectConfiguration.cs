using Lunatic.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lunatic.Infrastructure.Data.Configurations.Write {
	internal sealed class ProjectConfiguration : IEntityTypeConfiguration<Project>{
		public void Configure(EntityTypeBuilder<Project> builder) {
			builder.ToTable("Projects");
		}
	}
}
