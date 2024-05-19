using Lunatic.Application.Persistence;
using Task = Lunatic.Domain.Entities.Task;
using Lunatic.Domain.Entities;
using Lunatic.Domain.Utils;
using NSubstitute;
using Lunatic.Application.Features.Tasks.Commands.DeleteComment;

namespace Tests.Lunatic.Application.Features.Comments.Commands {
	public class DeleteTaskCommentTests {
        private static Result<User> user = User.Create("firstName", "lastName", "email@email.com", "username", "password", Role.USER);
        private static Result<Task> task = Task.Create(user.Value.Id, Guid.NewGuid(), "taskSection", "taskTitle", "testDescription", TaskPriority.LOW, DateTime.UtcNow, DateTime.UtcNow);
        private static Result<Comment> comment = Comment.Create(user.Value.Id, task.Value.Id, "commentContent");

        public DeleteTaskCommentTests() {
            task.Value.AddComment(comment.Value);
        }

        [Fact]
        public async void GivenDeleteTaskCommentCommand_WhenDelete_ThenSuccessResponse() {
            var taskRepository = Substitute.For<ITaskRepository>();
            var commentRepository = Substitute.For<ICommentRepository>();

            taskRepository.ExistsByIdAsync(task.Value.Id).Returns(true);
            commentRepository.ExistsByIdAsync(comment.Value.CommentId).Returns(true);
            taskRepository.FindByIdAsync(task.Value.Id).Returns(task);
            commentRepository.DeleteAsync(comment.Value.CommentId).Returns(comment);

            var command = new DeleteCommentCommand {
                TaskId = task.Value.Id,
                CommentId = comment.Value.CommentId
            };

            var commandHandler = new DeleteTaskCommentCommandHandler(taskRepository, commentRepository);

            var source = new CancellationTokenSource();

            var response = await commandHandler.Handle(command, source.Token);

            Assert.True(response.Success);
            Assert.Null(response.ValidationErrors);
        }
    }
}
