namespace Lunatic.UI.Models.ViewModels {
	public class ProjectViewModel {
		public Guid UserId { get; set; }
		public Guid TeamId { get; set; }
		public string Title { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
	}
}