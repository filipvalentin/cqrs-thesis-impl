
using Lunatic.Domain.Entities;


namespace Tests.Lunatic.Domain.Entities.CommentTests {
    public class CreateTests {
        public const string Content = "Lunatic Content";

        [Fact]
        public void GivenValidComment_WhenCreateComment_ThenSuccessResult() {
            // given

            // when
            var commentResult = Comment.Create(Guid.NewGuid(), Guid.NewGuid(), Content);

            // then
            Assert.True(commentResult.IsSuccess);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void GivenInvalidComment_WhenCreateComment_ThenFailureResult(string content) {
            // given

            // when
            var commentResult = Comment.Create(Guid.NewGuid(), Guid.NewGuid(), content);

            // then
            Assert.False(commentResult.IsSuccess);
        }

        [Fact]
        public void GivenInvalidComment_WhenCreateComment_ThenFailureResult_2() {
            // given

            // when
            var commentResult = Comment.Create(default, Guid.NewGuid(), Content);

            // then
            Assert.False(commentResult.IsSuccess);
        }

        [Fact]
        public void GivenInvalidComment_WhenCreateComment_ThenFailureResult_3() {
            // given

            // when
            var commentResult = Comment.Create(Guid.NewGuid(), default, Content);

            // then
            Assert.False(commentResult.IsSuccess);
        }
    }
}

