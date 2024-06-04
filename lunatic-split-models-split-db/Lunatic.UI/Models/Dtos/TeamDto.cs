namespace Lunatic.UI.Models.Dtos {
	public class TeamDto {
		public Guid TeamId { get; set; }
		public Guid OwnerId { get; set; }
		public string Name { get; set; } = string.Empty;
		public string? Description { get; set; }
		public List<Guid> MemberIds { get; set; } = []; //TODO! check if initialized already
		public List<Guid> ProjectIds { get; set; } = [];

	}
}
