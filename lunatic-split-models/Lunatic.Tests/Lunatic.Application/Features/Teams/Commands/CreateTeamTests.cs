using Lunatic.Application.Features.Teams.Commands.CreateTeam;
using Lunatic.Application.Persistence;
using Lunatic.Domain.Entities;
using Lunatic.Domain.Utils;
using NSubstitute;

namespace Tests.Lunatic.Application.Features.Teams.Commands
{
    public class CreateTeamTests
    {
        private Result<User> user = User.Create("firstName", "lastName", "email@email.com", "username", "password", Role.USER);

        [Fact]
        public async void GivenCreateTeamCommand_WhenCreate_Then_SuccessResponse()
        {
            // given
            var userRepository = Substitute.For<IUserRepository>();
            var teamRepository = Substitute.For<ITeamRepository>();

            userRepository.FindByIdAsync(user.Value.Id).Returns(user);
            userRepository.ExistsByIdAsync(user.Value.Id).Returns(true);

            var command = new CreateTeamCommand
            {
                Name = "TeamName",
                Description = "TeamDescription",
                UserId = user.Value.Id
            };

            var commandHandler = new CreateTeamCommandHandler(teamRepository, userRepository);
            var source = new CancellationTokenSource();

            // when
            var response = await commandHandler.Handle(command, source.Token);

            // then
            Assert.True(response.Success);
            Assert.NotNull(response.Team);
            Assert.Null(response.ValidationErrors);
			Assert.NotNull(teamRepository.FindByIdAsync(response.Team.TeamId));
        }

		[Fact]
		public async void GivenInvalidUserId_WhenCreate_Then_FailureResponse()
		{
			// given
			var userRepository = Substitute.For<IUserRepository>();
			var teamRepository = Substitute.For<ITeamRepository>();

			userRepository.FindByIdAsync(user.Value.Id).Returns(user);
			userRepository.ExistsByIdAsync(user.Value.Id).Returns(true);

			var command = new CreateTeamCommand
			{
				Name = "TeamName",
				Description = "TeamDescription",
				UserId = Guid.NewGuid()
			};

			var commandHandler = new CreateTeamCommandHandler(teamRepository, userRepository);
			var source = new CancellationTokenSource();

			// when
			var response = await commandHandler.Handle(command, source.Token);

			// then
			Assert.False(response.Success);
			Assert.Null(response.Team);
			Assert.NotNull(response.ValidationErrors);
		}

		[Fact]
		public async void GivenInvalidName_WhenCreate_Then_FailureResponse()
		{
			// given
			var userRepository = Substitute.For<IUserRepository>();
			var teamRepository = Substitute.For<ITeamRepository>();

			userRepository.FindByIdAsync(user.Value.Id).Returns(user);
			userRepository.ExistsByIdAsync(user.Value.Id).Returns(true);

			var command = new CreateTeamCommand
			{
				Name = "",
				Description = "TeamDescription",
				UserId = user.Value.Id
			};

			var commandHandler = new CreateTeamCommandHandler(teamRepository, userRepository);
			var source = new CancellationTokenSource();

			// when
			var response = await commandHandler.Handle(command, source.Token);

			// then
			Assert.False(response.Success);
			Assert.Null(response.Team);
			Assert.NotNull(response.ValidationErrors);
		}

		[Fact]
		public async void GivenInvalidDescription_WhenCreate_Then_FailureResponse()
		{
			// given
			var userRepository = Substitute.For<IUserRepository>();
			var teamRepository = Substitute.For<ITeamRepository>();

			userRepository.FindByIdAsync(user.Value.Id).Returns(user);
			userRepository.ExistsByIdAsync(user.Value.Id).Returns(true);

			var command = new CreateTeamCommand
			{
				Name = "TeamName",
				Description = "",
				UserId = user.Value.Id
			};

			var commandHandler = new CreateTeamCommandHandler(teamRepository, userRepository);
			var source = new CancellationTokenSource();

			// when
			var response = await commandHandler.Handle(command, source.Token);

			// then
			Assert.False(response.Success);
			Assert.Null(response.Team);
			Assert.NotNull(response.ValidationErrors);
		}
	}
}
