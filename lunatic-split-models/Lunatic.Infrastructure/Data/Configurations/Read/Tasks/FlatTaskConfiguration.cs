using Lunatic.Application.Models.ReadModels.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lunatic.Infrastructure.Data.Configurations.Read.Tasks {
	internal sealed class FlatTaskConfiguration : IEntityTypeConfiguration<FlatTaskReadModel> {
		public void Configure(EntityTypeBuilder<FlatTaskReadModel> builder) {
			builder.ToTable("Tasks");

			builder.HasKey(t => t.Id);

			builder.Property(t => t.ProjectId).HasColumnName(nameof(FlatTaskReadModel.ProjectId));
			builder.Property(t => t.CreatedByUserId).HasColumnName(nameof(FlatTaskReadModel.CreatedByUserId));
			builder.Property(t => t.Title).HasColumnName(nameof(FlatTaskReadModel.Title));
			builder.Property(t => t.Description).HasColumnName(nameof(FlatTaskReadModel.Description));
			builder.Property(t => t.TaskSectionCard).HasColumnName(nameof(FlatTaskReadModel.TaskSectionCard));
			builder.Property(t => t.Priority).HasColumnName(nameof(FlatTaskReadModel.Priority));
			builder.Property(t => t.Tags).HasColumnName(nameof(FlatTaskReadModel.Tags));
			builder.Property(t => t.Status).HasColumnName(nameof(FlatTaskReadModel.Status));
			builder.Property(t => t.AssigneeIds).HasColumnName(nameof(FlatTaskReadModel.AssigneeIds));
			builder.Property(t => t.PlannedStartDate).HasColumnName(nameof(FlatTaskReadModel.PlannedStartDate));
			builder.Property(t => t.PlannedEndDate).HasColumnName(nameof(FlatTaskReadModel.PlannedEndDate));
			builder.Property(t => t.StartedDate).HasColumnName(nameof(FlatTaskReadModel.StartedDate));
			builder.Property(t => t.EndedDate).HasColumnName(nameof(FlatTaskReadModel.EndedDate));


			builder.HasMany(t => t.Comments)
				.WithOne()
				.HasForeignKey("TaskId");

			builder.HasOne<TaskReadModel>()
				.WithOne()
				.HasForeignKey<FlatTaskReadModel>(t => t.Id);

			builder.Navigation(t => t.Comments)
				.AutoInclude();
		}
	}
}
