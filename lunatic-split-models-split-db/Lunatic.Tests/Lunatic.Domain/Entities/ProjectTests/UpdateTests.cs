
using Lunatic.Domain.Entities;


namespace Tests.Lunatic.Domain.Entities.ProjectTests {
    public class UpdateTests {
        public const string Title = "Lunatic";
        public const string Description = "Lunatic Description";

        [Fact]
        public void GivenProject_WhenUpdateProject_ThenUpdatedSuccessfully() {
            // given
            var project = Project.Create(Guid.NewGuid(), Guid.NewGuid(), Title, Description).Value;

            // when
            project.Update("Update" + Title, "Update" + Description);

            // then
            Assert.Equal("Update" + Title, project.Title);
            Assert.Equal("Update" + Description, project.Description);
        }
    }
}

