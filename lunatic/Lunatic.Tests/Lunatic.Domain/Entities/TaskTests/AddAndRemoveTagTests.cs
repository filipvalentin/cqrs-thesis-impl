
using Lunatic.Domain.Entities;
using Task = Lunatic.Domain.Entities.Task;


namespace Tests.Lunatic.Domain.Entities.TaskTests {
    public class AddAndRemoveTagTests {
        public const string Section = "Lunatic Section";
        public const string Title = "Lunatic";
        public const string Description = "Lunatic Description";
        public const TaskPriority Priority = TaskPriority.LOW;
        public const string Tag = "Lunatic Documentation";

        [Fact]
        public void GivenTaskWithProject_WhenAddAndRemoveProject_ThenProjectAddedAndRemovedSuccessfully() {
            // given
            var task = Task.Create(Guid.NewGuid(), Guid.NewGuid(), Section, Title, Description, Priority, DateTime.UtcNow, DateTime.UtcNow).Value;

            // when
            task.AddTag(Tag);
            task.RemoveTag(Tag);

            // then
            Assert.DoesNotContain(Tag, task.Tags);
        }
    }
}

