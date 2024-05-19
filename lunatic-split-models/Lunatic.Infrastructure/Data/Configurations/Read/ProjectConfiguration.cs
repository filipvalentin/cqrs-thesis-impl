using Lunatic.Application.Models.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lunatic.Infrastructure.Data.Configurations.Read {
	internal sealed class ProjectConfiguration : IEntityTypeConfiguration<ProjectReadModel>{
		public void Configure(EntityTypeBuilder<ProjectReadModel> builder) {
			builder.ToTable("Projects");
		}
	}
}
