namespace Lunatic.Application.Models.ReadModels {
	public class ProjectReadModel {
		public Guid Id { get; set; }
		public Guid CreatedByUserId { get; set; }
		public Guid TeamId { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public List<string> TaskSectionCards { get; set; } = [];
		public List<Guid> TaskIds { get; set; } = [];
	}

}
