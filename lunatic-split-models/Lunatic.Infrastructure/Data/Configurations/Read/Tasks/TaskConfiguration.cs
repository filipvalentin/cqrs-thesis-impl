using Lunatic.Application.Models.ReadModels.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lunatic.Infrastructure.Data.Configurations.Read.Tasks {
	internal sealed class TaskConfiguration : IEntityTypeConfiguration<TaskReadModel> {
		public void Configure(EntityTypeBuilder<TaskReadModel> builder) {
			builder.ToTable("Tasks");

			builder.HasKey(t => t.Id);
			builder.Property(t => t.ProjectId).HasColumnName("ProjectId");
			builder.Property(t => t.CreatedByUserId).HasColumnName("CreatedByUserId");
			builder.Property(t => t.TaskSectionCard).HasColumnName("TaskSectionCard");
			builder.Property(t => t.Title).HasColumnName("Title");
			builder.Property(t => t.Description).HasColumnName("Description");
			builder.Property(t => t.Priority).HasColumnName("Priority");
			builder.Property(t => t.Status).HasColumnName("Status");
			builder.Property(t => t.CommentIds).HasColumnName("CommentIds");
			builder.Property(t => t.AssigneeIds).HasColumnName("AssigneeIds");
			builder.Property(t => t.Tags).HasColumnName("Tags");
			builder.Property(t => t.PlannedStartDate).HasColumnName("PlannedStartDate");
			builder.Property(t => t.PlannedEndDate).HasColumnName("PlannedEndDate");
			builder.Property(t => t.StartedDate).HasColumnName("StartedDate");
			builder.Property(t => t.EndedDate).HasColumnName("EndedDate");
		}
	}
}
