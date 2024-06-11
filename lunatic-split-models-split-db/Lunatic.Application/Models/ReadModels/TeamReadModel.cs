namespace Lunatic.Application.Models.ReadModels {
	public class TeamReadModel {
		public Guid Id { get; set; }
		public Guid CreatedByUserId { get; set; }
		public string Name { get; set; }
		public string Description { get; set; } = "";
		public List<Guid> MemberIds { get; set; } = [];
		public List<Guid> ProjectIds { get; set; } = [];
	}
}
