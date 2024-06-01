using Lunatic.Domain.Entities;

namespace Lunatic.Domain.MLModel {
	public class DaysToCompleteTaskEntry {
		public Guid TaskId { get; set; }
		public int AssigneesCount { get; set; }
		public int DescriptionLength { get; set; }
		public int CommentsCount { get; set; }
		public double AverageCommentLength { get; set; }
		public TaskPriority Priority { get; set; }
	}
}
