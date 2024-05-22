using Lunatic.Application.Models.ReadModels.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lunatic.Infrastructure.Data.Configurations.Read.Tasks {
	internal sealed class CompoundTaskConfiguration : IEntityTypeConfiguration<CompoundTaskReadModel> {
		public void Configure(EntityTypeBuilder<CompoundTaskReadModel> builder) {
			builder.HasBaseType<BaseTaskReadModel>();


			// Assuming the Task entity has an Id property
			//builder.HasKey(t => t.Id);

			// Map other properties
			//builder.Property(t => t.Name).HasColumnName("Name");
			// Map other properties as needed

			// Configure the relationship with CommentReadModel
			builder.HasMany(t => t.Comments)
				   .WithOne()  // No navigation property on CommentReadModel
				   .HasForeignKey("TaskId");
		}
	}
}
