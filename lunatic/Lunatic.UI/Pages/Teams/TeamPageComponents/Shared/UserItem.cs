namespace Lunatic.UI.Pages.Teams.TeamPageComponents.Shared {
	public class UserItem {
		public Guid UserId { get; init; }
		public string Username { get; init; }
		// public string Avatar { get; init; }

		public UserItem(Guid userId, string username) {
			UserId = userId;
			Username = username;
		}
	}
}
