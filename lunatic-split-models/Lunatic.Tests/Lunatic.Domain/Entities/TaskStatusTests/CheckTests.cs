
using Lunatic.Domain.Entities;
using TaskStatus = Lunatic.Domain.Entities.TaskStatus;


namespace Tests.Lunatic.Domain.Entities.TaskStatusTests {
    public class CheckTests {
        [Theory]
        [InlineData(TaskStatus.CREATED)]
        [InlineData(TaskStatus.IN_PROGRESS)]
        [InlineData(TaskStatus.DONE)]
        public void GivenTaskStatus_WhenIsCreated_ThenSuccessfully(TaskStatus status) {
            // given

            // when
            var result = status.IsCreated();

            // then
            Assert.Equal(result, status == TaskStatus.CREATED);
        }

        [Theory]
        [InlineData(TaskStatus.CREATED)]
        [InlineData(TaskStatus.IN_PROGRESS)]
        [InlineData(TaskStatus.DONE)]
        public void GivenTaskStatus_WhenIsInProgress_ThenSuccessfully(TaskStatus status) {
            // given

            // when
            var result = status.IsInProgress();

            // then
            Assert.Equal(result, status == TaskStatus.IN_PROGRESS);
        }

        [Theory]
        [InlineData(TaskStatus.CREATED)]
        [InlineData(TaskStatus.IN_PROGRESS)]
        [InlineData(TaskStatus.DONE)]
        public void GivenTaskStatus_WhenIsDone_ThenSuccessfully(TaskStatus status) {
            // given

            // when
            var result = status.IsDone();

            // then
            Assert.Equal(result, status == TaskStatus.DONE);
        }
    }
}

