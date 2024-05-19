
using Lunatic.Domain.Entities;


namespace Tests.Lunatic.Domain.Entities.TeamTests {
    public class UpdateTests {
        public const string Name = "Lunatic";
        public const string Description = "Lunatic Description";

        [Fact]
        public void GivenTeam_WhenUpdateTeam_ThenUpdatedSuccessfully() {
            // given
            var team = Team.Create(Guid.NewGuid(), Name, Description).Value;

            // when
            team.Update("Update" + Name, "Update" + Description);

            // then
            Assert.Equal("Update" + Name, team.Name);
            Assert.Equal("Update" + Description, team.Description);
        }
    }
}

