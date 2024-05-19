using Lunatic.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lunatic.Infrastructure.Data.Configurations.Write {
	internal sealed class TeamConfiguration : IEntityTypeConfiguration<Team>{
		public void Configure(EntityTypeBuilder<Team> builder) {
			builder.ToTable("Teams");
		}
	}
}
