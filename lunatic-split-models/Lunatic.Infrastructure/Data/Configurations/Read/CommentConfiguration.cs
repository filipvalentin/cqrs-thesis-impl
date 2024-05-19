using Lunatic.Application.Models.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lunatic.Infrastructure.Data.Configurations.Read {
	internal sealed class CommentConfiguration : IEntityTypeConfiguration<CommentReadModel>{
		public void Configure(EntityTypeBuilder<CommentReadModel> builder) {
			builder.ToTable("Comments");
			builder.HasKey(c => c.CommentId);
		}
	}
}
