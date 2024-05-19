
using Lunatic.Domain.Entities;


namespace Tests.Lunatic.Domain.Entities.TeamTests {
    public class AddAndRemoveProjectTests {
        public const string TeamName = "Lunatic";
        public const string TeamDescription = "Lunatic Description";

        public const string ProjectTitle = "Lunatic";
        public const string ProjectDescription = "Lunatic Description";

        [Fact]
        public void GivenTeamWithProject_WhenAddAndRemoveProject_ThenProjectAddedAndRemovedSuccessfully() {
            // given
            var team = Team.Create(Guid.NewGuid(), TeamName, TeamDescription).Value;
            var project = Project.Create(Guid.NewGuid(), Guid.NewGuid(), ProjectTitle, ProjectDescription).Value;
            var projectId = Guid.NewGuid();

            // when
            //team.AddProject(project);
            team.AddProject(projectId);
            //team.RemoveProject(project);
            team.RemoveProject(projectId);

            // then
            Assert.DoesNotContain(project.Id, team.ProjectIds);
            Assert.DoesNotContain(projectId, team.ProjectIds);
        }
    }
}

