using Lunatic.Application.Persistence;
using Lunatic.Domain.Entities;
using Task = Lunatic.Domain.Entities.Task;
using Lunatic.Domain.Utils;
using NSubstitute;
using Lunatic.Application.Features.Projects.Commands.DeleteTask;

namespace Tests.Lunatic.Application.Features.Projects.Commands {
	public class DeleteProjectTaskTests {
        private static Result<Project> project = Project.Create(Guid.NewGuid(), Guid.NewGuid(), "projectTitle", "projectDescription");
        private static Result<Task> task = Task.Create(Guid.NewGuid(), Guid.NewGuid(), "taskSection", "taskTitle", "taskTitle", TaskPriority.LOW, DateTime.UtcNow, DateTime.UtcNow);

        public DeleteProjectTaskTests() {
            project.Value.AddTask(task.Value);
        }

        [Fact]
        public async void GivenCreateProjectTaskCommand_WhenCreate_Then_SuccessResponse() {
            var projectRepository = Substitute.For<IProjectRepository>();
            var taskRepository = Substitute.For<ITaskRepository>();

            taskRepository.ExistsByIdAsync(task.Value.Id).Returns(true);
            projectRepository.ExistsByIdAsync(project.Value.Id).Returns(true);
            projectRepository.FindByIdAsync(project.Value.Id).Returns(project);
            taskRepository.DeleteAsync(task.Value.Id).Returns(task);

            var command = new DeleteProjectTaskCommand {
                ProjectId = project.Value.Id,
                TaskId = task.Value.Id
            };

            var commandHandler = new DeleteTaskCommandHandler(projectRepository, taskRepository);

            var source = new CancellationTokenSource();

            var response = await commandHandler.Handle(command, source.Token);

            Assert.True(response.Success);
            Assert.Null(response.ValidationErrors);
        }

    }
}
