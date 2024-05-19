
using Lunatic.Domain.Entities;
using Task = Lunatic.Domain.Entities.Task;


namespace Tests.Lunatic.Domain.Entities.TaskTests {
    public class CreateTests {
        public const string Section = "Lunatic Section";
        public const string Title = "Lunatic";
        public const string Description = "Lunatic Description";
        public const TaskPriority Priority = TaskPriority.LOW;

        [Fact]
        public void GivenValidTask_WhenCreateTask_ThenSuccessResult() {
            // given

            // when
            var taskResult = Task.Create(Guid.NewGuid(), Guid.NewGuid(), Section, Title, Description, Priority, DateTime.UtcNow, DateTime.UtcNow);

            // then
            Assert.True(taskResult.IsSuccess);
        }

        [Theory]
        [InlineData(null, Title, Description)]
        [InlineData("", Title, Description)]
        [InlineData(Section, null, Description)]
        [InlineData(Section, Title, null)]
        [InlineData(Section, "", Description)]
        [InlineData(Section, Title, "")]
        public void GivenInvalidTask_WhenCreateTask_ThenFailureResult(string section, string title, string description) {
            // given

            // when
            var taskResult = Task.Create(Guid.NewGuid(), Guid.NewGuid(), section, title, description, Priority, DateTime.UtcNow, DateTime.UtcNow);

            // then
            Assert.False(taskResult.IsSuccess);
        }

        [Fact]
        public void GivenInvalidTask_WhenCreateTask_ThenFailureResult_2() {
            // given

            // when
            var taskResult = Task.Create(default, Guid.NewGuid(), Section, Title, Description, Priority, DateTime.UtcNow, DateTime.UtcNow);

            // then
            Assert.False(taskResult.IsSuccess);
        }

        [Fact]
        public void GivenInvalidTask_WhenCreateTask_ThenFailureResult_3() {
            // given

            // when
            var taskResult = Task.Create(Guid.NewGuid(), default, Section, Title, Description, Priority, DateTime.UtcNow, DateTime.UtcNow);

            // then
            Assert.False(taskResult.IsSuccess);
        }
    }
}

