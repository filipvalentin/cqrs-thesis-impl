using Lunatic.Application.Models.ReadModels.Tasks;
using Lunatic.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lunatic.Infrastructure.Data.Configurations.Read.Tasks {
	internal sealed class TaskDescriptionConfiguration : IEntityTypeConfiguration<TaskDescriptionReadModel> {
		public void Configure(EntityTypeBuilder<TaskDescriptionReadModel> builder) {
			builder.ToTable("Tasks");

			builder.HasKey(t => t.Id);

			builder.Property(t => t.TaskSectionCard).HasColumnName("TaskSectionCard");
			builder.Property(t => t.Title).HasColumnName("Title");
			builder.Property(t => t.Description).HasColumnName("Description");
			builder.Property(t => t.Priority).HasColumnName("Priority");
			builder.Property(t => t.Tags).HasColumnName("Tags");

			builder.HasOne<TaskReadModel>()
				.WithOne()
				.HasForeignKey<TaskDescriptionReadModel>(t => t.Id);
		}
	}
}
