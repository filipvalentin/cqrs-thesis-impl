
using FluentAssertions;
using Lunatic.Application.Features.Projects.Commands.CreateProjectTask;
using Lunatic.Application.Features.Projects.Commands.CreateTaskSectionCard;
using Lunatic.Application.Features.Projects.Commands.DeleteTask;
using Lunatic.Application.Features.Projects.Queries.GetAllTaskSections;
using Lunatic.Domain.Entities;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Tests.Lunatic.API.Base;


namespace Tests.Lunatic.API.Controllers {
	public class ProjectsControllerTests : BaseApplicationContextTests {
		private const string RequestUri = "/api/v1/projects";

		[Fact]
		public async void WhenCreateProjectTaskCommandHandlerIsCalled_ThenSuccess() {
			// Given
			string token = CreateToken();
			Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
			var command = new CreateProjectTaskCommand {
				UserId = Seed.Users.First().Id,
				ProjectId = Seed.Projects.First().Id,

				Section = Seed.TaskSection,

				Title = Seed.ProjectTitle,
				Description = Seed.ProjectDescription,

				Priority = TaskPriority.LOW,

				PlannedStartDate = DateTime.UtcNow,
				PlannedEndDate = DateTime.UtcNow,
			};

			// When
			var response = await Client.PostAsJsonAsync(RequestUri + "/" + Seed.Projects.First().Id + "/tasks", command);

			// Then
			response.EnsureSuccessStatusCode();
			var responseString = await response.Content.ReadAsStringAsync();
			var result = JsonConvert.DeserializeObject<CreateProjectTaskCommandResponse>(responseString);
			result?.Success.Should().BeTrue();
		}

		[Fact]
		public async void WhenCreateProjectTaskCommandHandlerIsCalled_ThenFailure() {
			// Given
			string token = CreateToken();
			Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
			var command = new CreateProjectTaskCommand {
				UserId = Seed.Users.First().Id,
				ProjectId = Seed.Projects.First().Id,

				Section = Seed.RandomTaskSection,

				Title = Seed.ProjectTitle,
				Description = Seed.ProjectDescription,

				Priority = TaskPriority.LOW,

				PlannedStartDate = DateTime.UtcNow,
				PlannedEndDate = DateTime.UtcNow,
			};

			// When
			var response = await Client.PostAsJsonAsync(RequestUri + "/" + Seed.Projects.First().Id + "/tasks", command);

			// Then
			var responseString = await response.Content.ReadAsStringAsync();
			var result = JsonConvert.DeserializeObject<CreateProjectTaskCommandResponse>(responseString);
			response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
			result?.Success.Should().BeFalse();
		}

		[Fact]
		public async void WhenCreateProjectTaskCommandHandlerIsCalled_ThenFailure1() {
			// Given
			string token = CreateToken();
			Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
			var command = new CreateProjectTaskCommand {
				UserId = Seed.Users.First().Id,
				ProjectId = Seed.Projects.First().Id,

				Section = Seed.RandomTaskSection,

				Title = "",
				Description = Seed.ProjectDescription,

				Priority = TaskPriority.LOW,

				PlannedStartDate = DateTime.UtcNow,
				PlannedEndDate = DateTime.UtcNow,
			};

			// When
			var response = await Client.PostAsJsonAsync(RequestUri + "/" + Seed.Projects.First().Id + "/tasks", command);

			// Then
			var responseString = await response.Content.ReadAsStringAsync();
			var result = JsonConvert.DeserializeObject<CreateProjectTaskCommandResponse>(responseString);
			response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
			result?.Success.Should().BeFalse();
		}

		[Fact]
		public async void WhenCreateProjectTaskCommandHandlerIsCalled_ThenFailure2() {
			// Given
			string token = CreateToken();
			Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
			var command = new CreateProjectTaskCommand {
				UserId = Seed.Users.First().Id,
				ProjectId = Seed.Projects.First().Id,

				Section = Seed.RandomTaskSection,

				Title = Seed.ProjectTitle,
				Description = "",

				Priority = TaskPriority.LOW,

				PlannedStartDate = DateTime.UtcNow,
				PlannedEndDate = DateTime.UtcNow,
			};

			// When
			var response = await Client.PostAsJsonAsync(RequestUri + "/" + Seed.Projects.First().Id + "/tasks", command);

			// Then
			var responseString = await response.Content.ReadAsStringAsync();
			var result = JsonConvert.DeserializeObject<CreateProjectTaskCommandResponse>(responseString);
			response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
			result?.Success.Should().BeFalse();
		}

		[Fact]
		public async void WhenCreateProjectTaskSectionCommandHandlerIsCalled_ThenSuccess() {
			// Given
			string token = CreateToken();
			Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
			var command = new CreateProjectTaskCommand {
				ProjectId = Seed.Projects.First().Id,
				Section = Seed.RandomTaskSection
			};

			// When
			var response = await Client.PostAsJsonAsync(RequestUri + "/" + Seed.Projects.First().Id + "/tasks/sections", command);

			// Then
			response.EnsureSuccessStatusCode();
			var responseString = await response.Content.ReadAsStringAsync();
			var result = JsonConvert.DeserializeObject<CreateTaskSectionCommandResponse>(responseString);
			result?.Success.Should().BeTrue();
		}

		[Fact]
		public async void WhenCreateProjectTaskSectionCommandHandlerIsCalled_ThenFailure() {
			// Given
			string token = CreateToken();
			Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
			var command = new CreateProjectTaskCommand {
				ProjectId = Seed.Projects.First().Id,
				Section = Seed.TaskSection
			};

			// When
			var response = await Client.PostAsJsonAsync(RequestUri + "/" + Seed.Projects.First().Id + "/tasks/sections", command);

			// Then
			var responseString = await response.Content.ReadAsStringAsync();
			var result = JsonConvert.DeserializeObject<CreateTaskSectionCommandResponse>(responseString);
			response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
			result?.Success.Should().BeFalse();
		}

		[Fact]
		public async void WhenCreateCommentEmotesCommandHandlerIsCalled_ThenFailure1() {
			// Given
			string token = CreateToken();
			Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
			var command = new CreateProjectTaskCommand {
				UserId = Seed.Users.First().Id,
				ProjectId = Seed.Projects.First().Id,

				Section = Seed.TaskSection,

				Title = "",
				Description = Seed.ProjectDescription,

				Priority = TaskPriority.LOW,

				PlannedStartDate = DateTime.UtcNow,
				PlannedEndDate = DateTime.UtcNow,
			};

			// When
			var response = await Client.PostAsJsonAsync(RequestUri + "/" + Seed.Projects.First().Id + "/tasks", command);

			// Then
			var responseString = await response.Content.ReadAsStringAsync();
			var result = JsonConvert.DeserializeObject<CreateProjectTaskCommandResponse>(responseString);
			response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
			result?.Success.Should().BeFalse();
		}

		[Fact]
		public async void WhenGetAllProjectTasksQueryHandlerIsCalled_ThenSuccess() {
			// Given && When
			var response = await Client.GetAsync(RequestUri + "/" + Seed.Projects.First().Id.ToString() + "/tasks");

			response.EnsureSuccessStatusCode();
			var responseString = await response.Content.ReadAsStringAsync();
			var result = JsonConvert.DeserializeObject<GetAllProjectTasksQueryResponse>(responseString);

			// Then
			result?.Tasks.Count().Should().Be(3);
		}

		[Fact]
		public async void WhenGetAllProjectTasksQueryHandlerIsCalled_ThenFailure() {
			// Given && When
			var response = await Client.GetAsync(RequestUri + "/" + Seed.RandomGuid + "/tasks");

			var responseString = await response.Content.ReadAsStringAsync();
			var result = JsonConvert.DeserializeObject<GetAllProjectTasksQueryResponse>(responseString);

			// Then
			response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
			result?.Success.Should().BeFalse();
		}

		[Fact]
		public async void WhenGetByIdProjectTaskQueryHandlerIsCalled_ThenSuccess() {
			// Given && When
			var response = await Client.GetAsync(RequestUri + "/tasks/" + Seed.Tasks.First().Id);

			response.EnsureSuccessStatusCode();
			var responseString = await response.Content.ReadAsStringAsync();
			var result = JsonConvert.DeserializeObject<GetByIdCompoundTaskQueryResponse>(responseString);

			// Then
			result?.Success.Should().BeTrue();
		}

		[Fact]
		public async void WhenGetByIdProjectTaskQueryHandlerIsCalled_ThenFailure() {
			// Given && When
			var response = await Client.GetAsync(RequestUri + "/tasks/" + Seed.RandomGuid);

			var responseString = await response.Content.ReadAsStringAsync();
			var result = JsonConvert.DeserializeObject<GetByIdCompoundTaskQueryResponse>(responseString);

			// Then
			response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
			result?.Success.Should().BeFalse();
		}

		[Fact]
		public async void WhenGetAllProjectTaskSectionsQueryHandlerIsCalled_ThenSuccess() {
			// Given && When
			var response = await Client.GetAsync(RequestUri + "/" + Seed.Projects.First().Id + "/tasks/sections");

			response.EnsureSuccessStatusCode();
			var responseString = await response.Content.ReadAsStringAsync();
			var result = JsonConvert.DeserializeObject<GetAllProjectTaskSectionsQueryResponse>(responseString);

			// Then
			result?.Success.Should().BeTrue();
		}

		[Fact]
		public async void WhenGetAllProjectTaskSectionsQueryHandlerIsCalled_ThenFailure() {
			// Given && When
			var response = await Client.GetAsync(RequestUri + "/" + Seed.RandomGuid + "/tasks/sections");

			var responseString = await response.Content.ReadAsStringAsync();
			var result = JsonConvert.DeserializeObject<GetAllProjectTaskSectionsQueryResponse>(responseString);

			// Then
			response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
			result?.Success.Should().BeFalse();
		}

		[Fact]
		public async void WhenDeleteProjectTaskSectionCommandHandlerIsCalled_ThenSuccess() {
			// Given && When
			var response = await Client.DeleteAsync(
				RequestUri + "/" + Seed.Projects.First().Id.ToString() + "/tasks/sections/" + Seed.TaskSection);

			response.EnsureSuccessStatusCode();
			var responseString = await response.Content.ReadAsStringAsync();
			var result = JsonConvert.DeserializeObject<DeleteTaskSectionCommandResponse>(responseString);

			// Then
			result?.Success.Should().BeTrue();
		}

		[Fact]
		public async void WhenDeleteProjectTaskSectionCommandHandlerIsCalled_ThenFailure() {
			// Given && When
			var response = await Client.DeleteAsync(
				RequestUri + "/" + Seed.Projects.First().Id.ToString() + "/tasks/sections/" + Seed.RandomTaskSection);

			var responseString = await response.Content.ReadAsStringAsync();
			var result = JsonConvert.DeserializeObject<DeleteTaskSectionCommandResponse>(responseString);

			// Then
			response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
			result?.Success.Should().BeFalse();
		}

		[Fact]
		public async void WhenDeleteProjectTaskCommandHandlerIsCalled_ThenSuccess() {
			// Given && When
			var response = await Client.DeleteAsync(
				RequestUri + "/" + Seed.Projects.First().Id.ToString() + "/tasks/" + Seed.Tasks.First().Id.ToString());

			response.EnsureSuccessStatusCode();
			var responseString = await response.Content.ReadAsStringAsync();
			var result = JsonConvert.DeserializeObject<DeleteTaskCommandResponse>(responseString);

			// Then
			result?.Success.Should().BeTrue();
		}

		[Fact]
		public async void WhenDeleteProjectTaskCommandHandlerIsCalled_ThenFailure() {
			// Given && When
			var response = await Client.DeleteAsync(
				RequestUri + "/" + Seed.RandomGuid + "/tasks/" + Seed.Tasks.First().Id.ToString());

			var responseString = await response.Content.ReadAsStringAsync();
			var result = JsonConvert.DeserializeObject<DeleteTaskCommandResponse>(responseString);

			// Then
			response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
			result?.Success.Should().BeFalse();
		}

		[Fact]
		public async void WhenDeleteProjectTaskCommandHandlerIsCalled_ThenFailure1() {
			// Given && When
			var response = await Client.DeleteAsync(RequestUri + "/" + Seed.Projects.First().Id.ToString() + "/tasks/" + Seed.RandomGuid);

			var responseString = await response.Content.ReadAsStringAsync();
			var result = JsonConvert.DeserializeObject<DeleteTaskCommandResponse>(responseString);

			// Then
			response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
			result?.Success.Should().BeFalse();
		}

		[Fact]
		public async void WhenDeleteProjectTaskCommandHandlerIsCalled_ThenFailure2() {
			// Given && When
			var response = await Client.DeleteAsync(RequestUri + "/" + Seed.RandomGuid + "/tasks/" + Seed.RandomGuid);

			var responseString = await response.Content.ReadAsStringAsync();
			var result = JsonConvert.DeserializeObject<DeleteTaskCommandResponse>(responseString);

			// Then
			response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
			result?.Success.Should().BeFalse();
		}
	}
}
