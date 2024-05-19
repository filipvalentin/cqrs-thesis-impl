
using Lunatic.Domain.Utils;

namespace Lunatic.Domain.Entities {
	public class Project : AuditableEntity {
		private Project(Guid createdByUserId, Guid teamId, string title, string description) : base(createdByUserId) {
			Id = Guid.NewGuid();
			TeamId = teamId;
			Title = title;
			Description = description;
		}

		public Guid Id { get; private set; }
		public Guid TeamId { get; private set; }
		public string Title { get; private set; }
		public string Description { get; private set; }
		public List<string> TaskSectionCards { get; private set; } = new List<string>();
		public List<Guid> TaskIds { get; private set; } = new List<Guid>();

		public static Result<Project> Create(Guid createdByUserId, Guid teamId, string title, string description) {
			if (createdByUserId == default) {
				return Result<Project>.Failure("Created by User Id is required.");
			}

			if (teamId == default) {
				return Result<Project>.Failure("Team Id is required.");
			}

			if (string.IsNullOrWhiteSpace(title)) {
				return Result<Project>.Failure("Title is required.");
			}

			if (string.IsNullOrWhiteSpace(description)) {
				return Result<Project>.Failure("Description is required.");
			}

			return Result<Project>.Success(new Project(createdByUserId, teamId, title, description));
		}

		public void Update(string title, string description) {
			Title = title;
			Description = description;
			LastModifiedDate = DateTime.UtcNow;
		}

		public void AddTaskSectionCard(string section) => TaskSectionCards.Add(section);
		public void RemoveTaskSectionCard(string section) => TaskSectionCards.Remove(section);

		public void AddTask(Task task) => TaskIds.Add(task.Id);
		public void AddTask(Guid taskId) => TaskIds.Add(taskId);
		public void RemoveTask(Task task) => TaskIds.Remove(task.Id);
		public void RemoveTask(Guid taskId) => TaskIds.Remove(taskId);
	}
}
