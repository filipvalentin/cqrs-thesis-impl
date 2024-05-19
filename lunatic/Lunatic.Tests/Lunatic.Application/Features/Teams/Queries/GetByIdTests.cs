
using Lunatic.Domain.Entities;
using Lunatic.Domain.Utils;
using Lunatic.Application.Persistence;
using Lunatic.Application.Features.Teams.Queries.GetById;
using NSubstitute;


namespace Tests.Lunatic.Application.Features.Teams.Queries {
    public class GetByIdTests {
        private static Result<Team> team = Team.Create(Guid.NewGuid(), "teamName", "teamDescription");

        [Fact]
        public async void GivenGetByIdUserQuery_WhenGetByIdUser_ThenSuccessResponse() {
            // given
            var teamRepository = Substitute.For<ITeamRepository>();
            teamRepository.FindByIdAsync(team.Value.Id).Returns(team);

            var query = new GetByIdTeamQuery(team.Value.Id);
            var queryHandler = new GetByIdTeamQueryHandler(teamRepository);
            var source = new CancellationTokenSource();

            // when
            var response = await queryHandler.Handle(query, source.Token);

            // then
            Assert.True(response.Success);
            Assert.NotNull(response.Team);
            Assert.Null(response.ValidationErrors);
        }

        [Fact]
        public async void GivenGetAllUserTeamsComamnd_WhenGetAllUserTeams_ThenFailureResponse() {
            // given
            var teamRepository = Substitute.For<ITeamRepository>();
            teamRepository.FindByIdAsync(team.Value.Id).Returns(Result<Team>.Failure("Lunatic Entity Not Found"));

            var query = new GetByIdTeamQuery(team.Value.Id);
            var queryHandler = new GetByIdTeamQueryHandler(teamRepository);
            var source = new CancellationTokenSource();

            // when
            var response = await queryHandler.Handle(query, source.Token);

            // then
            Assert.False(response.Success);
            Assert.Null(response.Team);
            Assert.NotNull(response.ValidationErrors);
        }
    }
}

