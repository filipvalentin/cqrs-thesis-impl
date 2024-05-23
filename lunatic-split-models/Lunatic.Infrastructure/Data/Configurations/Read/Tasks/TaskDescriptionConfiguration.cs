using Lunatic.Application.Models.ReadModels.Tasks;
using Lunatic.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lunatic.Infrastructure.Data.Configurations.Read.Tasks {
	internal sealed class TaskDescriptionConfiguration : IEntityTypeConfiguration<TaskDescriptionReadModel> {
		public void Configure(EntityTypeBuilder<TaskDescriptionReadModel> builder) {
			builder.ToTable("Tasks");

			builder.HasKey(t => t.Id);
			builder.Property(t => t.Title).HasColumnName(nameof(TaskDescriptionReadModel.Title));
			builder.Property(t => t.Description).HasColumnName(nameof(TaskDescriptionReadModel.Description));
			builder.Property(t => t.TaskSectionCard).HasColumnName(nameof(TaskDescriptionReadModel.TaskSectionCard));
			builder.Property(t => t.Priority).HasColumnName(nameof(TaskDescriptionReadModel.Priority));
			builder.Property(t => t.Tags).HasColumnName(nameof(TaskDescriptionReadModel.Tags));

			builder.HasOne<TaskReadModel>()
				.WithOne()
				.HasForeignKey<TaskDescriptionReadModel>(t => t.Id);
		}
	}
}
