using FluentAssertions;
using Lunatic.Application.Features.Tasks.Commands.CreateComment;
using Lunatic.Application.Features.Tasks.Commands.DeleteComment;
using Lunatic.Application.Features.Tasks.Queries.GetAllTaskComments;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using Tests.Lunatic.API.Base;

namespace Tests.Lunatic.API.Controllers {
	public class TasksControllerTests : BaseApplicationContextTests {
		private const string RequestURL = "/api/v1/tasks";

		[Fact]
		public async void WhenGetAllTaskCommentsIsCalled_ThenSuccess() {
			Guid taskId = Seed.Tasks.First().Id;
			var response = await Client.GetAsync($"{RequestURL}/{taskId}/comments");

			response.EnsureSuccessStatusCode();
			var responseString = await response.Content.ReadAsStringAsync();
			var result = JsonConvert.DeserializeObject<GetAllTaskCommentsQueryResponse>(responseString);

			result.Comments.Count().Should().Be(1);
		}

		[Fact]
		public async void WhenGetCommentByIdIsCalled_ThenSuccess() {
			Guid commentId = Seed.Comments.First().CommentId;
			var response = await Client.GetAsync($"{RequestURL}/comments/{commentId}");

			response.EnsureSuccessStatusCode();
			var responseString = await response.Content.ReadAsStringAsync();
			var result = JsonConvert.DeserializeObject<GetByIdTaskCommentQueryResponse>(responseString);

			result.Comment.CommentId.Should().Be(commentId);
		}

		[Fact]
		public async void WhenCreateTaskCommentIsCalled_ThenSuccess() {
			var userId = Seed.Users.First().Id;
			var taskId = Seed.Tasks.First().Id;

			var token = CreateToken();
			Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
			var comment = new CreateCommentCommand {
				UserId = userId,
				TaskId = taskId,
				Content = "Test"
			};

			var response = await Client.PostAsJsonAsync($"{RequestURL}/{taskId}/comments", comment);

			response.EnsureSuccessStatusCode();
			var responseString = await response.Content.ReadAsStringAsync();
			var result = JsonConvert.DeserializeObject<CreateCommentCommandResponse>(responseString);
			result?.Should().NotBeNull();
			response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
			result?.Comment.Content.Should().Be("Test");
		}

		[Fact]
		public async void WhenDeleteTaskCommentIsCalled_ThenSuccess() {
			Guid taskId = Seed.Tasks.First().Id;
			Guid commentId = Seed.Comments.First().CommentId;

			var response = await Client.DeleteAsync($"{RequestURL}/{taskId}/comments/{commentId}");

			response.EnsureSuccessStatusCode();
			var responseString = await response.Content.ReadAsStringAsync();
			var result = JsonConvert.DeserializeObject<DeleteCommentCommandResponse>(responseString);

			result.Success.Should().BeTrue();
		}

		[Fact]
		public async void WhenDeleteTaskCommentIsCalledWithWrongTaskId_ThenFailure() {
			Guid taskId = Guid.NewGuid();
			Guid commentId = Seed.Comments.First().CommentId;

			var response = await Client.DeleteAsync($"{RequestURL}/{taskId}/comments/{commentId}");

			response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
			var responseString = await response.Content.ReadAsStringAsync();
			var result = JsonConvert.DeserializeObject<DeleteCommentCommandResponse>(responseString);

			result.Success.Should().BeFalse();
		}

		[Fact]
		public async void WhenDeleteTaskCommentIsCalledWithWrongCommentId_ThenFailure() {
			Guid taskId = Seed.Tasks.First().Id;
			Guid commentId = Guid.NewGuid();

			var response = await Client.DeleteAsync($"{RequestURL}/{taskId}/comments/{commentId}");

			response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
			var responseString = await response.Content.ReadAsStringAsync();
			var result = JsonConvert.DeserializeObject<DeleteCommentCommandResponse>(responseString);

			result.Success.Should().BeFalse();
		}

		[Fact]
		public async void WhenCreateTaskCommentIsCalledWithWrongUserId_ThenFailure() {
			var userId = Guid.NewGuid();
			var taskId = Seed.Tasks.First().Id;

			var token = CreateToken();
			Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
			var comment = new CreateCommentCommand {
				UserId = userId,
				TaskId = taskId,
				Content = "Test"
			};

			var response = await Client.PostAsJsonAsync($"{RequestURL}/{taskId}/comments", comment);

			response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
			var responseString = await response.Content.ReadAsStringAsync();
			var result = JsonConvert.DeserializeObject<CreateCommentCommandResponse>(responseString);
			result?.Should().NotBeNull();
			result?.Success.Should().BeFalse();
		}

		[Fact]
		public async void WhenCreateTaskCommentIsCalledWithWrongTaskId_ThenFailure() {
			var userId = Seed.Users.First().Id;
			var taskId = Guid.NewGuid();

			var token = CreateToken();
			Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
			var comment = new CreateCommentCommand {
				UserId = userId,
				TaskId = taskId,
				Content = "Test"
			};

			var response = await Client.PostAsJsonAsync($"{RequestURL}/{taskId}/comments", comment);

			response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
			var responseString = await response.Content.ReadAsStringAsync();
			var result = JsonConvert.DeserializeObject<CreateCommentCommandResponse>(responseString);
			result?.Should().NotBeNull();
			result?.Success.Should().BeFalse();
		}

		[Fact]
		public async void WhenCreateTaskCommentIsCalledWithWrongParameters_ThenFailure() {
			var userId = Seed.Users.First().Id;
			var taskId = Seed.Tasks.First().Id;

			var token = CreateToken();
			Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
			var comment = new CreateCommentCommand {
				UserId = userId,
				TaskId = taskId,
				Content = ""
			};

			var response = await Client.PostAsJsonAsync($"{RequestURL}/{taskId}/comments", comment);

			response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
			var responseString = await response.Content.ReadAsStringAsync();
			var result = JsonConvert.DeserializeObject<CreateCommentCommandResponse>(responseString);
			result?.Should().NotBeNull();
			result?.Success.Should().BeFalse();
		}

		private static string CreateToken() {

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
