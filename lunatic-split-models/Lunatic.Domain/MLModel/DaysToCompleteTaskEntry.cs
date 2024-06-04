using Lunatic.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Lunatic.Domain.MLModel {
	public class DaysToCompleteTaskEntry {
		public string? TaskId { get; set; }
		public int AssigneesCount { get; set; }
		public int DescriptionLength { get; set; }
		public int CommentsCount { get; set; }
		public double AverageCommentLength { get; set; }
		public TaskPriority Priority { get; set; }
		public int DaysTookToComplete { get; set; }
		public int ExpectedDaysToComplete { get; set; }
	}
}
