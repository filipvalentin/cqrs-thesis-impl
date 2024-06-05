using Lunatic.Domain.Entities;

namespace Lunatic.Application.Models.ReadModels {
	public class UserReadModel {
		public Guid UserId { get; set; } = default!;
		public string FirstName { get; set; } = default!;
		public string LastName { get; set; } = default!;
		public string Email { get; set; } = default!;
		public string Username { get; set; } = default!;
		public Role Role { get; set; } = default!;
		public List<Guid> TeamIds { get; set; } = default!;
	}
}
