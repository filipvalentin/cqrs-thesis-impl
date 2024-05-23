using Lunatic.Application.Models.ReadModels.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lunatic.Infrastructure.Data.Configurations.Read.Tasks {
	internal sealed class TaskConfiguration : IEntityTypeConfiguration<TaskReadModel> {
		public void Configure(EntityTypeBuilder<TaskReadModel> builder) {
			builder.ToTable("Tasks");

			builder.HasKey(t => t.Id);

			builder.Property(t => t.ProjectId).HasColumnName(nameof(TaskReadModel.ProjectId));
			builder.Property(t => t.CreatedByUserId).HasColumnName(nameof(TaskReadModel.CreatedByUserId));
			builder.Property(t => t.Title).HasColumnName(nameof(TaskReadModel.Title));
			builder.Property(t => t.Description).HasColumnName(nameof(TaskReadModel.Description));
			builder.Property(t => t.TaskSectionCard).HasColumnName(nameof(TaskReadModel.TaskSectionCard));
			builder.Property(t => t.Priority).HasColumnName(nameof(TaskReadModel.Priority));
			builder.Property(t => t.Tags).HasColumnName(nameof(TaskReadModel.Tags));
			builder.Property(t => t.Status).HasColumnName(nameof(TaskReadModel.Status));
			builder.Property(t => t.CommentIds).HasColumnName(nameof(TaskReadModel.CommentIds));
			builder.Property(t => t.AssigneeIds).HasColumnName(nameof(TaskReadModel.AssigneeIds));
			builder.Property(t => t.PlannedStartDate).HasColumnName(nameof(TaskReadModel.PlannedStartDate));
			builder.Property(t => t.PlannedEndDate).HasColumnName(nameof(TaskReadModel.PlannedEndDate));
			builder.Property(t => t.StartedDate).HasColumnName(nameof(TaskReadModel.StartedDate));
			builder.Property(t => t.EndedDate).HasColumnName(nameof(TaskReadModel.EndedDate));
		}
	}
}
