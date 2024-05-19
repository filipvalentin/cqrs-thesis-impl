namespace Lunatic.UI.Models.ViewModels {
	public class TeamViewModel {
		public Guid UserId { get; set; }
		public string Name { get; set; } = null!;
		public string? Description { get; set; }
		public List<Guid> MemberIds { get; set; } = null!;
		public List<Guid>? ProjectIds { get; set; }
	}
}