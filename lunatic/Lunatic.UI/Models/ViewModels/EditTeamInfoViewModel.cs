namespace Lunatic.UI.Models.ViewModels {
	public class EditTeamInfoViewModel {
		public Guid TeamId { get; set; }
		public string Name { get; set; } = null!;
		public string? Description { get; set; }
	}
}