
using Lunatic.Domain.Entities;


namespace Tests.Lunatic.Domain.Entities.ProjectTests {
    public class CreateTests {
        public const string Title = "Lunatic";
        public const string Description = "Lunatic Description";

        [Fact]
        public void GivenValidProject_WhenCreateProject_ThenSuccessResult() {
            // given

            // when
            var projectResult = Project.Create(Guid.NewGuid(), Guid.NewGuid(), Title, Description);

            // then
            Assert.True(projectResult.IsSuccess);
        }

        [Theory]
        [InlineData(null, Description)]
        [InlineData(Title, null)]
        [InlineData("", Description)]
        [InlineData(Title, "")]
        public void GivenInvalidProject_WhenCreateProject_ThenFailureResult(string title, string description) {
            // given

            // when
            var projectResult = Project.Create(Guid.NewGuid(), Guid.NewGuid(), title, description);

            // then
            Assert.False(projectResult.IsSuccess);
        }

        [Fact]
        public void GivenInvalidProject_WhenCreateProject_ThenFailureResult_2() {
            // given

            // when
            var projectResult = Project.Create(default, Guid.NewGuid(), Title, Description);

            // then
            Assert.False(projectResult.IsSuccess);
        }

        [Fact]
        public void GivenInvalidProject_WhenCreateProject_ThenFailureResult_3() {
            // given

            // when
            var projectResult = Project.Create(Guid.NewGuid(), default, Title, Description);

            // then
            Assert.False(projectResult.IsSuccess);
        }
    }
}

