
using Lunatic.Domain.Entities;
using Task = Lunatic.Domain.Entities.Task;


namespace Tests.Lunatic.Domain.Entities.TaskTests {
    public class AddAndRemoveAssigneeTests {
        public const string TaskSection = "Lunatic Section";
        public const string TaskTitle = "Lunatic";
        public const string TaskDescription = "Lunatic Description";

        public const string UserFirstName = "John";
        public const string UserLastName = "Smith";
        public const string UserEmail = "smith@gmail.com";
        public const string UserUsername = "smith";
        public const string UserPassword = "String123#";
        public const Role UserRole = Role.USER;

        [Fact]
        public void GivenTaskWithUser_WhenAddAndRemoveMember_ThenMemberAddedAndRemovedSuccessfully() {
            // given
            var task = Task.Create(Guid.NewGuid(), Guid.NewGuid(), TaskSection, TaskTitle, TaskDescription, TaskPriority.LOW, DateTime.UtcNow, DateTime.UtcNow).Value;
            var user = User.Create(UserFirstName, UserLastName, UserEmail, UserUsername, UserPassword, UserRole).Value;
            var userId = Guid.NewGuid();

            // when
            task.AddAssignee(user);
            task.AddAssignee(userId);
            task.RemoveAssignee(user);
            task.RemoveAssignee(userId);

            // then
            Assert.DoesNotContain(user.Id, task.AssigneeIds);
            Assert.DoesNotContain(userId, task.AssigneeIds);
        }
    }
}

