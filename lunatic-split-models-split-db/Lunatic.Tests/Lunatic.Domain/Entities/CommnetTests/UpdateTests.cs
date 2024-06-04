
using Lunatic.Domain.Entities;


namespace Tests.Lunatic.Domain.Entities.CommentTests {
    public class UpdateTests {
        public const string Content = "Lunatic Content";

        [Fact]
        public void GivenComment_WhenUpdateComment_ThenUpdatedSuccessfully() {
            // given
            var comment = Comment.Create(Guid.NewGuid(), Guid.NewGuid(), Content).Value;

            // when
            comment.Update("Update" + Content);

            // then
            Assert.Equal("Update" + Content, comment.Content);
        }
    }
}

