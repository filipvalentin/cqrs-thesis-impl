
using Lunatic.Domain.Entities;


namespace Tests.Lunatic.Domain.Entities.TeamTests {
    public class CreateTests {
        public const string Name = "Lunatic";
        public const string Description = "Lunatic Description";

        [Fact]
        public void GivenValidTeam_WhenCreateTeam_ThenSuccessResult() {
            // given

            // when
            var teamResult = Team.Create(Guid.NewGuid(), Name, Description);

            // then
            Assert.True(teamResult.IsSuccess);
        }

        [Theory]
        [InlineData(null, Description)]
        [InlineData(Name, null)]
        [InlineData("", Description)]
        [InlineData(Name, "")]
        public void GivenInvalidTeam_WhenCreateTeam_ThenFailureResult(string name, string description) {
            // given

            // when
            var teamResult = Team.Create(Guid.NewGuid(), name, description);

            // then
            Assert.False(teamResult.IsSuccess);
        }

        [Fact]
        public void GivenInvalidTeam_WhenCreateTeam_ThenFailureResult_2() {
            // given

            // when
            var teamResult = Team.Create(default, Name, Description);

            // then
            Assert.False(teamResult.IsSuccess);
        }
    }
}

