using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FluentAssertions;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Tests.Lunatic.API.Base;
using Lunatic.Application.Features.Teams.Commands.CreateTeam;
using System.Net.Http.Json;
using Lunatic.Application.Features.Teams.Queries.GetById;
using Lunatic.Application.Features.Teams.Commands.DeleteTeam;
using Lunatic.Application.Features.Teams.Queries.GetAllMembers;
using Lunatic.Application.Features.Teams.Commands.AddTeamMember;

namespace Tests.Lunatic.API.Controllers {
	public class TeamsControllerTests : BaseApplicationContextTests
	{
		private const string RequestURL = "/api/v1/teams";

		[Fact]
		public async void WhenGetAllTeamsIsCalled_ThenSuccess()
		{
			var response = await Client.GetAsync(RequestURL);

			response.EnsureSuccessStatusCode();
			var responseString = await response.Content.ReadAsStringAsync();
			var result = JsonConvert.DeserializeObject<GetAllTeamsQueryResponse>(responseString);

			result.Teams.Count().Should().Be(2);
		}

		[Fact]
		public async void WhenGetTeamByIdIsCalled_ThenSuccess()
		{
			Guid teamId = Seed.Teams.First().Id;
			var response = await Client.GetAsync($"{RequestURL}/{teamId}");

			response.EnsureSuccessStatusCode();
			var responseString = await response.Content.ReadAsStringAsync();
			var result = JsonConvert.DeserializeObject<GetByIdTeamQueryResponse>(responseString);

			result.Team.TeamId.Should().Be(teamId);
		}

		[Fact]
		public async void WhenDeleteTeamIsCalled_ThenSuccess()
		{
			Guid teamId = Seed.Teams.First().Id;
			var response = await Client.DeleteAsync($"{RequestURL}/{teamId}");

			response.EnsureSuccessStatusCode();
			var responseString = await response.Content.ReadAsStringAsync();
			var result = JsonConvert.DeserializeObject<DeleteTeamCommandResponse>(responseString);

			result.Success.Should().BeTrue();
		}

		[Fact]
		public async void WhenGetAllMembersIsCalled_ThenSuccess()
		{
			Guid teamId = Seed.Teams.First().Id;
			var response = await Client.GetAsync($"{RequestURL}/{teamId}/members");

			response.EnsureSuccessStatusCode();
			var responseString = await response.Content.ReadAsStringAsync();
			var result = JsonConvert.DeserializeObject<GetAllTeamMembersQueryResponse>(responseString);

			result.Members.Count().Should().Be(1);
		}

		[Fact]
		public async void WhenAddTeamMemberIsCalled_ThenSuccess()
		{
			Guid teamId = Seed.Teams.First().Id;
			var userId = Seed.Users.First().Id;

			var token = CreateToken();
			Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

			var command = new AddTeamMemberCommand
			{
				TeamId = teamId,
				UserId = userId
			};

			var response = await Client.PostAsJsonAsync($"{RequestURL}/{teamId}/members", command);
			
			response.EnsureSuccessStatusCode();
			var responseString = await response.Content.ReadAsStringAsync();
			var result = JsonConvert.DeserializeObject<AddTeamMemberCommandResponse>(responseString);
			result?.Should().NotBeNull();
			response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
			result?.Team.TeamId.Should().Be(teamId);
		}

		[Fact]
		public async void WhenDeleteTeamMemberIsCalled_ThenSuccess()
		{
			Guid teamId = Seed.Teams.First().Id;
			Guid userId = Seed.Users.Last().Id;
			var response = await Client.DeleteAsync($"{RequestURL}/{teamId}/members/{userId}");

			response.EnsureSuccessStatusCode();
			var responseString = await response.Content.ReadAsStringAsync();
			var result = JsonConvert.DeserializeObject<DeleteTeamCommandResponse>(responseString);

			result.Success.Should().BeTrue();
		}

		[Fact]
		public async void WhenCreateTeamIsCalled_ThenSuccess()
		{
			var userId = Seed.Users.First().Id;

			var token = CreateToken();
			Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
			var team = new CreateTeamCommand
			{
				Name = "Test",
				Description = "Test",
				UserId = userId
			};

			var response = await Client.PostAsJsonAsync(RequestURL, team);

			response.EnsureSuccessStatusCode();
			var responseString = await response.Content.ReadAsStringAsync();
			var result = JsonConvert.DeserializeObject<CreateTeamCommandResponse>(responseString);
			result?.Should().NotBeNull();
			response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
			result?.Team.Name.Should().Be("Test");
		}

		[Fact]
		public async void WhenGetTeamByIdIsCalledWithWrongId_ThenFailure()
		{
			Guid teamId = Guid.NewGuid();
			var response = await Client.GetAsync($"{RequestURL}/{teamId}");

			response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
			var responseString = await response.Content.ReadAsStringAsync();
			var result = JsonConvert.DeserializeObject<GetByIdTeamQueryResponse>(responseString);

			result.Success.Should().BeFalse();
		}

		[Fact]
		public async void WhenDeleteTeamIsCalledWithWrongId_ThenFailure()
		{
			Guid teamId = Guid.NewGuid();
			var response = await Client.DeleteAsync($"{RequestURL}/{teamId}");

			response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
			var responseString = await response.Content.ReadAsStringAsync();
			var result = JsonConvert.DeserializeObject<DeleteTeamCommandResponse>(responseString);

			result.Success.Should().BeFalse();
		}

		[Fact]
		public async void WhenGetAllMembersIsCalledWithWrongId_ThenFailure()
		{
			Guid teamId = Guid.NewGuid();
			var response = await Client.GetAsync($"{RequestURL}/{teamId}/members");

			response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
			var responseString = await response.Content.ReadAsStringAsync();
			var result = JsonConvert.DeserializeObject<GetAllTeamMembersQueryResponse>(responseString);

			result.Success.Should().BeFalse();
		}

		[Fact]
		public async void WhenAddTeamMemberIsCalledWithWrongTeamId_ThenFailure()
		{
			Guid teamId = Guid.NewGuid();
			var userId = Seed.Users.First().Id;

			var token = CreateToken();
			Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

			var command = new AddTeamMemberCommand
			{
				TeamId = teamId,
				UserId = userId
			};

			var response = await Client.PostAsJsonAsync($"{RequestURL}/{teamId}/members", command);

			response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
			var responseString = await response.Content.ReadAsStringAsync();
			var result = JsonConvert.DeserializeObject<AddTeamMemberCommandResponse>(responseString);
			result?.Success.Should().BeFalse();
		}

		[Fact]
		public async void WhenAddTeamMemberIsCalledWithWrongUserId_ThenFailure()
		{
			Guid teamId = Seed.Teams.First().Id;
			Guid userId = Guid.NewGuid();

			var token = CreateToken();
			Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

			var command = new AddTeamMemberCommand
			{
				TeamId = teamId,
				UserId = userId
			};

			var response = await Client.PostAsJsonAsync($"{RequestURL}/{teamId}/members", command);

			response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
			var responseString = await response.Content.ReadAsStringAsync();
			var result = JsonConvert.DeserializeObject<AddTeamMemberCommandResponse>(responseString);
			result?.Success.Should().BeFalse();
		}

		[Fact]
		public async void WhenDeleteTeamMemberIsCalledWithWrongTeamId_ThenFailure()
		{
			Guid teamId = Guid.NewGuid();
			Guid userId = Seed.Users.Last().Id;
			var response = await Client.DeleteAsync($"{RequestURL}/{teamId}/members/{userId}");

			response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
			var responseString = await response.Content.ReadAsStringAsync();
			var result = JsonConvert.DeserializeObject<DeleteTeamCommandResponse>(responseString);

			result.Success.Should().BeFalse();
		}

		[Fact]
		public async void WhenDeleteTeamMemberIsCalledWithWrongUserId_ThenFailure()
		{
			Guid teamId = Seed.Teams.First().Id;
			Guid userId = Guid.NewGuid();
			var response = await Client.DeleteAsync($"{RequestURL}/{teamId}/members/{userId}");

			response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
			var responseString = await response.Content.ReadAsStringAsync();
			var result = JsonConvert.DeserializeObject<DeleteTeamCommandResponse>(responseString);

			result.Success.Should().BeFalse();
		}

		[Fact]
		public async void WhenCreateTeamIsCalledWithWrongUserId_ThenFailure()
		{
			var userId = Guid.NewGuid();

			var token = CreateToken();
			Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
			var team = new CreateTeamCommand
			{
				Name = "Test",
				Description = "Test",
				UserId = userId
			};

			var response = await Client.PostAsJsonAsync(RequestURL, team);

			response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
			var responseString = await response.Content.ReadAsStringAsync();
			var result = JsonConvert.DeserializeObject<CreateTeamCommandResponse>(responseString);
			result?.Success.Should().BeFalse();
		}

		[Theory]
		[InlineData("Test", "")]
		[InlineData("", "Test")]
		public async void WhenCreateTeamIsCalledWithWrongParameters_ThenFailure(string name, string description)
		{
			var userId = Seed.Users.First().Id;

			var token = CreateToken();
			Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
			var team = new CreateTeamCommand
			{
				Name = name,
				Description = description,
				UserId = userId
			};

			var response = await Client.PostAsJsonAsync(RequestURL, team);

			response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
			var responseString = await response.Content.ReadAsStringAsync();
			var result = JsonConvert.DeserializeObject<CreateTeamCommandResponse>(responseString);
			result?.Success.Should().BeFalse();
		}

		private static string CreateToken()
		{

			return JwtTokenProvider.JwtSecurityTokenHandler.WriteToken(
			new JwtSecurityToken(
				JwtTokenProvider.Issuer,
				JwtTokenProvider.Issuer,
				new List<Claim> { new(ClaimTypes.Role, "User"), new("department", "Security") },
				expires: DateTime.Now.AddMinutes(30),
				signingCredentials: JwtTokenProvider.SigningCredentials
			));
		}
	}
}
