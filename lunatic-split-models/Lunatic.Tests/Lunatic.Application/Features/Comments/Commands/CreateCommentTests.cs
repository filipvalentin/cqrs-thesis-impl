using Lunatic.Application.Persistence;
using Task = Lunatic.Domain.Entities.Task;
using Lunatic.Domain.Entities;
using Lunatic.Domain.Utils;
using NSubstitute;
using Lunatic.Application.Features.Tasks.Commands.CreateComment;

namespace Tests.Lunatic.Application.Features.Comments.Commands {
	public class CreateTaskCommentTests {
        private static Result<User> user = User.Create("firstName", "lastName", "email@email.com", "username", "password", Role.USER);
        private static Result<Task> task = Task.Create(user.Value.Id, Guid.NewGuid(), "taskSection", "taskTitle", "testDescription", TaskPriority.LOW, DateTime.UtcNow, DateTime.UtcNow);
        private static Result<Comment> comment = Comment.Create(user.Value.Id, task.Value.Id, "commentContent");

        [Fact]
        public async void GivenCreateProjectTaskCommand_WhenCreate_ThenSuccessResponse() {
            var userRepository = Substitute.For<IUserRepository>();
            var taskRepository = Substitute.For<ITaskRepository>();
            var commentRepository = Substitute.For<ICommentRepository>();

            userRepository.ExistsByIdAsync(user.Value.Id).Returns(true);
            taskRepository.ExistsByIdAsync(task.Value.Id).Returns(true);
            taskRepository.FindByIdAsync(task.Value.Id).Returns(task);

            var command = new CreateCommentCommand {
                UserId = user.Value.Id,
                TaskId = task.Value.Id,

                Content = "Test"
            };

            var commandHandler = new CreateCommentCommandHandler(taskRepository, commentRepository, userRepository);

            var source = new CancellationTokenSource();

            var response = await commandHandler.Handle(command, source.Token);

            Assert.True(response.Success);
            Assert.NotNull(response.Comment);
            Assert.Null(response.ValidationErrors);
        }
    }
}
