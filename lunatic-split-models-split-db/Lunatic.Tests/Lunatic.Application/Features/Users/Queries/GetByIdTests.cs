
using Lunatic.Domain.Entities;
using Lunatic.Domain.Utils;
using Lunatic.Application.Persistence;
using Lunatic.Application.Features.Users.Queries.GetById;
using NSubstitute;


namespace Tests.Lunatic.Application.Features.Users.Queries {
    public class GetByIdTests {
        private Result<User> user = User.Create("firstName", "lastName", "email@email.com", "username", "password", Role.USER);

        [Fact]
        public async void GivenGetByIdUserQuery_WhenGetByIdUser_ThenSuccessResponse() {
            // given
            var userRepository = Substitute.For<IUserRepository>();
            userRepository.FindByIdAsync(user.Value.Id).Returns(user);

            var query = new GetByIdUserQuery(user.Value.Id);
            var queryHandler = new GetByIdUserQueryHandler(userRepository);
            var source = new CancellationTokenSource();

            // when
            var response = await queryHandler.Handle(query, source.Token);

            // then
            Assert.True(response.Success);
            Assert.NotNull(response.User);
            Assert.Null(response.ValidationErrors);
        }

        [Fact]
        public async void GivenGetAllUserTeamsComamnd_WhenGetAllUserTeams_ThenFailureResponse() {
            // given
            var userRepository = Substitute.For<IUserRepository>();
            userRepository.FindByIdAsync(user.Value.Id).Returns(Result<User>.Failure("Lunatic Entity Not Found"));

            var query = new GetByIdUserQuery(user.Value.Id);
            var queryHandler = new GetByIdUserQueryHandler(userRepository);
            var source = new CancellationTokenSource();

            // when
            var response = await queryHandler.Handle(query, source.Token);

            // then
            Assert.False(response.Success);
            Assert.Null(response.User);
            Assert.NotNull(response.ValidationErrors);
        }
    }
}

