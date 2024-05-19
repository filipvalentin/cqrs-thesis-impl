
using Lunatic.Domain.Utils;
using Lunatic.Domain.Entities;
using Lunatic.Application.Persistence;
using Lunatic.Application.Features.Users.Commands.DeleteUser;
using NSubstitute;


namespace Tests.Lunatic.Application.Features.Users.Commands {
    public class DeleteUserTests {
        private static Result<User> user = User.Create("firstName", "lastName", "email@email.com", "username", "password", Role.USER);

        [Fact]
        public async void GivenCreateUserComamnd_WhenCreate_ThenSuccessResponse() {
            // given
            var userRepository = Substitute.For<IUserRepository>();
            userRepository.DeleteAsync(user.Value.Id).Returns(user);
            var command = new DeleteUserCommand {
                UserId = user.Value.Id
            };
            var commandHandler = new DeleteUserCommandHandler(userRepository);
            var source = new CancellationTokenSource();

            // when
            var response = await commandHandler.Handle(command, source.Token);

            // then
            Assert.True(response.Success);
            Assert.Null(response.ValidationErrors);
        }

        [Fact]
        public async void GivenDeleteUserComamnd_WhenDelete_ThenSuccessResponse() {
            // given
            var userRepository = Substitute.For<IUserRepository>();
            userRepository.DeleteAsync(user.Value.Id).Returns(Result<User>.Failure($"Entity with id {user.Value.Id} not found"));
            var command = new DeleteUserCommand {
                UserId = user.Value.Id
            };
            var commandHandler = new DeleteUserCommandHandler(userRepository);
            var source = new CancellationTokenSource();

            // when
            var response = await commandHandler.Handle(command, source.Token);

            // then
            Assert.False(response.Success);
            Assert.NotNull(response.ValidationErrors);
        }
    }
}

