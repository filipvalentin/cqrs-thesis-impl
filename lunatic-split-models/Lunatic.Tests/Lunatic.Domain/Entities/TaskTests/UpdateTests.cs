
using Lunatic.Domain.Entities;
using Task = Lunatic.Domain.Entities.Task;


namespace Tests.Lunatic.Domain.Entities.TaskTests {
	public class UpdateTests {
        public const string Section = "Lunatic Section";
        public const string Title = "Lunatic";
        public const string Description = "Lunatic Description";
        public DateTime PlannedStartDate = DateTime.UtcNow;
        public DateTime PlannedEndDate = DateTime.UtcNow;

        [Fact]
        public void GivenTask_WhenUpdateTask_ThenUpdatedSuccessfully() {
            // given
            var task = Task.Create(Guid.NewGuid(), Guid.NewGuid(), Section, Title, Description, TaskPriority.LOW, DateTime.UtcNow, DateTime.UtcNow).Value;

            // when
            task.Update("Update" + Title, "Update" + Description, TaskPriority.MEDIUM, PlannedStartDate, PlannedEndDate);

            // then
            Assert.Equal("Update" + Title, task.Title);
            Assert.Equal("Update" + Description, task.Description);
            Assert.Equal(TaskPriority.MEDIUM, task.Priority);
            Assert.Equal(PlannedStartDate, task.PlannedStartDate);
            Assert.Equal(PlannedEndDate, task.PlannedEndDate);
        }
    }
}

