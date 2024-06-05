namespace Lunatic.Application.Features.Users.Queries.GetUsernameMatches {
	public class UsernameMatch {
		public Guid UserId { get; set; }
		public string Username { get; set; } = default!;
	}
}
