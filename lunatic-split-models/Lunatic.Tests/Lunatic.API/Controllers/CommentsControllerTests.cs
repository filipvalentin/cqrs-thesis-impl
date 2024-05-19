
using FluentAssertions;
using Tests.Lunatic.API.Base;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http.Json;


namespace Tests.Lunatic.API.Controllers {
	public class CommentsControllerTests : BaseApplicationContextTests {
        private const string RequestUri = "/api/v1/comments";

        [Fact]
        public async void WhenCreateCommentEmotesCommandHandlerIsCalled_ThenSuccess() {
            // Given
            string token = CreateToken();
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var command = new CreateCommentEmoteCommand {
                UserId = Seed.Users.First().UserId,
                CommentId = Seed.Comments.First().CommentId,
                Emote = Emote.SMILE
            };

            // When
            var response = await Client.PostAsJsonAsync(RequestUri + "/" + Seed.Comments.First().CommentId + "/emotes", command);

            // Then
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<CreateCommentEmoteCommandResponse>(responseString);
            result?.Success.Should().BeTrue();
        }

        [Fact]
        public async void WhenCreateCommentEmotesCommandHandlerIsCalled_ThenFailure() {
            // Given
            string token = CreateToken();
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var command = new CreateCommentEmoteCommand {
                UserId = Seed.Users.First().UserId,
                CommentId = Guid.Parse(Seed.RandomGuid),
                Emote = Emote.SMILE
            };

            // When
            var response = await Client.PostAsJsonAsync(RequestUri + "/" + Seed.Comments.First().CommentId + "/emotes", command);

            // Then
            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<CreateCommentEmoteCommandResponse>(responseString);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            result?.Success.Should().BeFalse();
        }

        [Fact]
        public async void WhenCreateCommentEmotesCommandHandlerIsCalled_ThenFailure1() {
            // Given
            string token = CreateToken();
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var command = new CreateCommentEmoteCommand {
                UserId = Guid.Parse(Seed.RandomGuid),
                CommentId = Seed.Comments.First().CommentId,
                Emote = Emote.SMILE
            };

            // When
            var response = await Client.PostAsJsonAsync(RequestUri + "/" + Seed.Comments.First().CommentId + "/emotes", command);

            // Then
            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<CreateCommentEmoteCommandResponse>(responseString);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            result?.Success.Should().BeFalse();
        }

        [Fact]
        public async void WhenCreateCommentEmotesCommandHandlerIsCalled_ThenFailure2() {
            // Given
            string token = CreateToken();
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var command = new CreateCommentEmoteCommand {
                UserId = Guid.Parse(Seed.RandomGuid),
                CommentId = Guid.Parse(Seed.RandomGuid),
                Emote = Emote.SMILE
            };

            // When
            var response = await Client.PostAsJsonAsync(RequestUri + "/" + Seed.Comments.First().CommentId + "/emotes", command);

            // Then
            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<CreateCommentEmoteCommandResponse>(responseString);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            result?.Success.Should().BeFalse();
        }

        [Fact]
        public async void WhenGetAllCommentEmotesQueryHandlerIsCalled_ThenSuccess() {
            // Given && When
            var response = await Client.GetAsync(RequestUri + "/" + Seed.Comments.First().CommentId.ToString() + "/emotes");

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<GetAllCommentEmotesQueryResponse>(responseString);

            // Then
            result?.CommentEmotes.Count().Should().Be(1);
        }

        [Fact]
        public async void WhenGetAllCommentEmotesQueryHandlerIsCalled_ThenFailure() {
            // Given && When
            var response = await Client.GetAsync(RequestUri + "/"+ Seed.RandomGuid + "/emotes");

            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<GetAllCommentEmotesQueryResponse>(responseString);

            // Then
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
            result?.Success.Should().BeFalse();
        }

        [Fact]
        public async void WhenDeleteCommentEmoteCommandHandlerIsCalled_ThenSuccess() {
            // Given && When
            var response = await Client.DeleteAsync(RequestUri + "/" + Seed.Comments.First().CommentId.ToString() + "/emotes/" + Seed.CommentEmotes.First().CommentEmoteId.ToString());

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<DeleteCommentEmoteCommandResponse>(responseString);

            // Then
            result?.Success.Should().BeTrue();
        }

        [Fact]
        public async void WhenDeleteCommentEmoteCommandHandlerIsCalled_ThenFailure() {
            // Given && When
            var response = await Client.DeleteAsync(RequestUri + "/" + Seed.RandomGuid + "/emotes/" + Seed.CommentEmotes.First().CommentEmoteId.ToString());

            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<DeleteCommentEmoteCommandResponse>(responseString);

            // Then
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
            result?.Success.Should().BeFalse();
        }

        [Fact]
        public async void WhenDeleteCommentEmoteCommandHandlerIsCalled_ThenFailure1() {
            // Given && When
            var response = await Client.DeleteAsync(RequestUri + "/" + Seed.Comments.First().CommentId.ToString() + "/emotes/" + Seed.RandomGuid);

            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<GetAllCommentEmotesQueryResponse>(responseString);

            // Then
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
            result?.Success.Should().BeFalse();
        }

        [Fact]
        public async void WhenDeleteCommentEmoteCommandHandlerIsCalled_ThenFailure2() {
            // Given && When
            var response = await Client.DeleteAsync(RequestUri + "/" + Seed.RandomGuid + "/emotes/" + Seed.RandomGuid);

            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<GetAllCommentEmotesQueryResponse>(responseString);

            // Then
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
            result?.Success.Should().BeFalse();
        }
    }
}
