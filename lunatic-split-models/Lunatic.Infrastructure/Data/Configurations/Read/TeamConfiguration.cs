using Lunatic.Application.Models.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lunatic.Infrastructure.Data.Configurations.Read {
	internal sealed class TeamConfiguration : IEntityTypeConfiguration<TeamReadModel>{
		public void Configure(EntityTypeBuilder<TeamReadModel> builder) {
			builder.ToTable("Teams");
		}
	}
}
