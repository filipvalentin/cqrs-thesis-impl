using Lunatic.Application.Models.ReadModels.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lunatic.Infrastructure.Data.Configurations.Read
{
    internal sealed class BaseTaskConfiguration : IEntityTypeConfiguration<BaseTaskReadModel> {
		public void Configure(EntityTypeBuilder<BaseTaskReadModel> builder) {
			builder.ToTable("Tasks");
		}
	}
}
