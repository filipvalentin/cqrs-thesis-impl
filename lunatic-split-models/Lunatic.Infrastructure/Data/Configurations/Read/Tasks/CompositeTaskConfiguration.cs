using Lunatic.Application.Models.ReadModels.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lunatic.Infrastructure.Data.Configurations.Read.Tasks {
	internal sealed class CompositeTaskConfiguration : IEntityTypeConfiguration<CompositeTaskReadModel> {
		public void Configure(EntityTypeBuilder<CompositeTaskReadModel> builder) {
			builder.ToTable("Tasks");

			builder.HasKey(t => t.Id);

			builder.Property(t => t.ProjectId).HasColumnName(nameof(CompositeTaskReadModel.ProjectId));
			builder.Property(t => t.CreatedByUserId).HasColumnName(nameof(CompositeTaskReadModel.CreatedByUserId));
			builder.Property(t => t.Title).HasColumnName(nameof(CompositeTaskReadModel.Title));
			builder.Property(t => t.Description).HasColumnName(nameof(CompositeTaskReadModel.Description));
			builder.Property(t => t.TaskSectionCard).HasColumnName(nameof(CompositeTaskReadModel.TaskSectionCard));
			builder.Property(t => t.Priority).HasColumnName(nameof(CompositeTaskReadModel.Priority));
			builder.Property(t => t.Tags).HasColumnName(nameof(CompositeTaskReadModel.Tags));
			builder.Property(t => t.Status).HasColumnName(nameof(CompositeTaskReadModel.Status));
			builder.Property(t => t.Comments).HasColumnName(nameof(CompositeTaskReadModel.Comments));
			builder.Property(t => t.AssigneeIds).HasColumnName(nameof(CompositeTaskReadModel.AssigneeIds));
			builder.Property(t => t.PlannedStartDate).HasColumnName(nameof(CompositeTaskReadModel.PlannedStartDate));
			builder.Property(t => t.PlannedEndDate).HasColumnName(nameof(CompositeTaskReadModel.PlannedEndDate));
			builder.Property(t => t.StartedDate).HasColumnName(nameof(CompositeTaskReadModel.StartedDate));
			builder.Property(t => t.EndedDate).HasColumnName(nameof(CompositeTaskReadModel.EndedDate));


			builder.HasMany(t => t.Comments)
				.WithOne()
				.HasForeignKey("TaskId");

			builder.HasOne<TaskReadModel>()
				.WithOne()
				.HasForeignKey<CompositeTaskReadModel>(t => t.Id);

			builder.Navigation(t => t.Comments)
				.AutoInclude();
		}
	}
}
