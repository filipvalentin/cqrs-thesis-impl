using Lunatic.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lunatic.Infrastructure.Data.Configurations.Write {
	internal sealed class CommentConfiguration : IEntityTypeConfiguration<Comment>{
		public void Configure(EntityTypeBuilder<Comment> builder) {
			builder.ToTable("Comments");
		}
	}
}
