
using Lunatic.Domain.Entities;
using Task = Lunatic.Domain.Entities.Task;


namespace Tests.Lunatic.Domain.Entities.TaskTests {
    public class AddAndRemoveCommentTests {
        public const string TaskSection = "Lunatic Section";
        public const string TaskTitle = "Lunatic";
        public const string TaskDescription = "Lunatic Description";

        public const string CommentContent = "Lunatic";

        [Fact]
        public void GivenTaskWithProject_WhenAddAndRemoveProject_ThenProjectAddedAndRemovedSuccessfully() {
            // given
            var task = Task.Create(Guid.NewGuid(), Guid.NewGuid(), TaskSection, TaskTitle, TaskDescription, TaskPriority.LOW, DateTime.UtcNow, DateTime.UtcNow).Value;
            var comment = Comment.Create(Guid.NewGuid(), Guid.NewGuid(), CommentContent).Value;
            var commentId = Guid.NewGuid();

            // when
            task.AddComment(comment);
            task.AddComment(commentId);
            task.RemoveComment(comment);
            task.RemoveComment(commentId);

            // then
            Assert.DoesNotContain(comment.CommentId, task.CommentIds);
            Assert.DoesNotContain(commentId, task.CommentIds);
        }
    }
}

