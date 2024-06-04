//
// using Lunatic.Domain.Entities;
// using Task = Lunatic.Domain.Entities.Task;
// using TaskStatus = Lunatic.Domain.Entities.TaskStatus;
//
//
// namespace Tests.Lunatic.Domain.Entities.TaskTests {
//     public class TaskStatusTests {
//         public const string Section = "Lunatic Section";
//         public const string Title = "Lunatic";
//         public const string Description = "Lunatic Description";
//         public const TaskPriority Priority = TaskPriority.LOW;
//
//         [Fact]
//         public void GivenTaskWithCreatedStatus_WhenMarkAsInProgress_ThenMarkSuccessfully() {
//             // given
//             var task = Task.Create(Guid.NewGuid(), Guid.NewGuid(), Section, Title, Description, Priority, DateTime.UtcNow, DateTime.UtcNow).Value;
//
//             // when
//             task.MarkAsInProgress();
//
//             // then
//             Assert.Equal(TaskStatus.IN_PROGRESS, task.Status);
//         }
//
//         [Fact]
//         public void GivenTaskWithCreatedStatus_WhenMarkAsDone_ThenMarkSuccessfully() {
//             // given
//             var task = Task.Create(Guid.NewGuid(), Guid.NewGuid(), Section, Title, Description, Priority, DateTime.UtcNow, DateTime.UtcNow).Value;
//
//             // when
//             task.MarkAsDone();
//
//             // then
//             Assert.Equal(TaskStatus.DONE, task.Status);
//         }
//
//         [Fact]
//         public void GivenTaskWithInProgressStatus_WhenMarkAsInProgress_ThenThrowException() {
//             // given
//             var task = Task.Create(Guid.NewGuid(), Guid.NewGuid(), Section, Title, Description, Priority, DateTime.UtcNow, DateTime.UtcNow).Value;
//             task.Update(task.Title, task.Description, task.Priority, TaskStatus.IN_PROGRESS, DateTime.UtcNow, DateTime.UtcNow);
//
//             // when
//             Action when = () => task.MarkAsInProgress();
//
//             // then
//             Assert.Throws<InvalidActionException>(when);
//         }
//
//         [Fact]
//         public void GivenTaskWithInProgressStatus_WhenMarkAsDone_ThenMarkSuccessfully() {
//             // given
//             var task = Task.Create(Guid.NewGuid(), Guid.NewGuid(), Section, Title, Description, Priority, DateTime.UtcNow, DateTime.UtcNow).Value;
//             task.Update(task.Title, task.Description, task.Priority, TaskStatus.IN_PROGRESS, DateTime.UtcNow, DateTime.UtcNow);
//
//             // when
//             task.MarkAsDone();
//
//             // then
//             Assert.Equal(TaskStatus.DONE, task.Status);
//         }
//
//         [Fact]
//         public void GivenTaskWithDoneStatus_WhenMarkAsInProgress_ThenMarkSuccessfully() {
//             // given
//             var task = Task.Create(Guid.NewGuid(), Guid.NewGuid(), Section, Title, Description, Priority, DateTime.UtcNow, DateTime.UtcNow).Value;
//             task.Update(task.Title, task.Description, task.Priority, TaskStatus.DONE, DateTime.UtcNow, DateTime.UtcNow);
//
//             // when
//             task.MarkAsInProgress();
//
//             // then
//             Assert.Equal(TaskStatus.IN_PROGRESS, task.Status);
//         }
//
//         [Fact]
//         public void GivenTaskWithDoneStatus_WhenMarkAsDone_ThenMarkSuccessfully() {
//             // given
//             var task = Task.Create(Guid.NewGuid(), Guid.NewGuid(), Section, Title, Description, Priority, DateTime.UtcNow, DateTime.UtcNow).Value;
//             task.Update(task.Title, task.Description, task.Priority, TaskStatus.DONE, DateTime.UtcNow, DateTime.UtcNow);
//
//             // when
//             task.MarkAsDone();
//
//             // then
//             Assert.Equal(TaskStatus.DONE, task.Status);
//         }
//     }
// }
//
