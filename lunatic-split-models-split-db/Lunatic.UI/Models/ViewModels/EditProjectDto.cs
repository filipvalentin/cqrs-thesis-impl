﻿namespace Lunatic.UI.Models.ViewModels {
	public class EditProjectDto {
		public Guid ProjectId { get; set; }
		public string Title { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
	}
}