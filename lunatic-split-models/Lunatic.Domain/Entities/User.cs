
using Lunatic.Domain.Utils;

namespace Lunatic.Domain.Entities {
	public class User {
		private User(string firstName, string lastName, string email, string username, Role role) {
			Id = Guid.NewGuid();
			RegisteredAt = DateTime.UtcNow;

			FirstName = firstName;
			LastName = lastName;
			Email = email;
			Username = username;
			Role = role;
		}

		public Guid Id { get; private set; }
		public DateTime RegisteredAt { get; private set; }

		public string FirstName { get; private set; }
		public string LastName { get; private set; }
		public string Email { get; private set; }
		public string Username { get; private set; }
		public Role Role { get; private set; }

		public List<Guid> TeamIds { get; private set; } = new List<Guid>();

		public static Result<User> Create(string firstName, string lastName, string email, string username, string password, Role role) {
			if (string.IsNullOrWhiteSpace(firstName)) {
				return Result<User>.Failure("First name is required.");
			}

			if (string.IsNullOrWhiteSpace(lastName)) {
				return Result<User>.Failure("Last name is required.");
			}

			if (string.IsNullOrWhiteSpace(email)) {
				return Result<User>.Failure("Email is required.");
			}

			if (string.IsNullOrWhiteSpace(username)) {
				return Result<User>.Failure("Username is required.");
			}

			if (string.IsNullOrWhiteSpace(password)) {
				return Result<User>.Failure("Password is required.");
			}

			return Result<User>.Success(new User(firstName, lastName, email, username, role));
		}

		public void Update(string firstName, string lastName, string email) {
			FirstName = firstName;
			LastName = lastName;
			Email = email;
		}

		//public void AddTeam(Team team) => TeamIds.Add(team.Id);
		public void AddTeam(Guid teamId) => TeamIds.Add(teamId);
		//public void RemoveTeam(Team team) => TeamIds.Remove(team.Id);
		public void RemoveTeam(Guid teamId) => TeamIds.Remove(teamId);
	}
}
