namespace Lunatic.UI.Models.ViewModels {
	public class UpdateTeamInfoViewModel {
		public string TeamId { get; set; } = null!;
		public string Name { get; set; } = null!;
		public string? Description { get; set; }
	}
}