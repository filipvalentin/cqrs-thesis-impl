
using Lunatic.Domain.Entities;
using Lunatic.Domain.Utils;
using Lunatic.Application.Persistence;
using NSubstitute;


namespace Tests.Lunatic.Application.Features.Users.Queries {
	public class GetAllTeamsTests {
        private Result<User> user = User.Create("firstName", "lastName", "email@email.com", "username", "password", Role.USER);

        [Fact]
        public async void GivenGetAllUserTeamsComamnd_WhenGetAllUserTeams_ThenSuccessResponse() {
            // given
            var userRepository = Substitute.For<IUserRepository>();
            var teamRepository = Substitute.For<ITeamRepository>();
            userRepository.FindByIdAsync(user.Value.Id).Returns(user);

            var query = new GetAllUserTeamsQuery(user.Value.Id);
            var queryHandler = new GetAllUserTeamsQueryHandler(userRepository, teamRepository);
            var source = new CancellationTokenSource();

            // when
            var response = await queryHandler.Handle(query, source.Token);

            // then
            Assert.True(response.Success);
            Assert.NotNull(response.Teams);
            Assert.Null(response.ValidationErrors);
        }

        [Fact]
        public async void GivenGetAllUserTeamsComamnd_WhenGetAllUserTeams_ThenFailureResponse() {
            // given
            var userRepository = Substitute.For<IUserRepository>();
            var userId = Guid.NewGuid();
            userRepository.FindByIdAsync(userId).Returns(Result<User>.Failure("Lunatic Entity Not Found"));
            var teamRepository = Substitute.For<ITeamRepository>();
            var query = new GetAllUserTeamsQuery(userId);
            var queryHandler = new GetAllUserTeamsQueryHandler(userRepository, teamRepository);
            var source = new CancellationTokenSource();

            // when
            var response = await queryHandler.Handle(query, source.Token);

            // then
            Assert.False(response.Success);
            Assert.Null(response.Teams);
            Assert.NotNull(response.ValidationErrors);
        }
    }
}

