using Lunatic.Application.Models.ReadModels.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lunatic.Infrastructure.Data.Configurations.Read.Tasks {
	internal sealed class TaskConfiguration : IEntityTypeConfiguration<TaskReadModel> {
		public void Configure(EntityTypeBuilder<TaskReadModel> builder) {
			builder.HasBaseType<BaseTaskReadModel>();
			builder.Property(t => t.CommentIds).HasColumnName("CommentIds");
		}
	}
}
