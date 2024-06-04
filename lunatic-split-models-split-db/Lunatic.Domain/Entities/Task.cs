
using Lunatic.Domain.Utils;

namespace Lunatic.Domain.Entities {
	public class Task : AuditableEntity {
		private Task(Guid createdByUserId, Guid projectId, string taskSectionCard, string title, string description, TaskPriority priority, DateTime plannedStartDate, DateTime plannedEndDate) : base(createdByUserId) {
			Id = Guid.NewGuid();
			ProjectId = projectId;
			TaskSectionCard = taskSectionCard;
			Title = title;
			Description = description;
			Priority = priority;
			Status = TaskStatus.CREATED;
			PlannedStartDate = plannedStartDate;
			PlannedEndDate = plannedEndDate;
		}

		public Guid Id { get; private set; }
		public Guid ProjectId { get; private set; }
		public string TaskSectionCard { get; private set; }
		public string Title { get; private set; }
		public string Description { get; private set; }
		public TaskPriority Priority { get; private set; }
		public TaskStatus Status { get; private set; }
		public List<string> Tags { get; private set; } = new List<string>();
		public List<Guid> CommentIds { get; private set; } = new List<Guid>();
		public List<Guid> AssigneeIds { get; private set; } = new List<Guid>();
		public DateTime PlannedStartDate { get; private set; }
		public DateTime PlannedEndDate { get; private set; }
		public DateTime? StartedDate { get; private set; }
		public DateTime? EndedDate { get; private set; }

		public static Result<Task> Create(Guid createdByUserId, Guid projectId, string section, string title, string description, TaskPriority priority, DateTime plannedStartDate, DateTime plannedEndDate) {
			if (createdByUserId == default) {
				return Result<Task>.Failure("Created by User Id is required.");
			}

			if (projectId == default) {
				return Result<Task>.Failure("Project Id is required.");
			}

			if (string.IsNullOrWhiteSpace(section)) {
				return Result<Task>.Failure("Section is required.");
			}

			if (string.IsNullOrWhiteSpace(title)) {
				return Result<Task>.Failure("Title is required.");
			}

			if (string.IsNullOrWhiteSpace(description)) {
				return Result<Task>.Failure("Description is required.");
			}

			return Result<Task>.Success(new Task(createdByUserId, projectId, section, title, description, priority, plannedStartDate, plannedEndDate));
		}

		public void Update(string title, string description, TaskPriority priority, DateTime plannedStartDate, DateTime plannedEndDate) {
			Title = title;
			Description = description;
			Priority = priority;
			PlannedStartDate = plannedStartDate;
			PlannedEndDate = plannedEndDate;
			LastModifiedDate = DateTime.UtcNow;
		}

		public void UpdateLists(List<string> tags, List<Guid> assigneeIds) {
			Tags = tags;
			AssigneeIds = assigneeIds;
		}

		public void SetSection(string section) {
			TaskSectionCard = section;
		}

		public void SetStatus(TaskStatus status) {
			Status = status;

			if (Status.IsInProgress()) {
				StartedDate = DateTime.UtcNow;
				EndedDate = null;
			}
			if (Status.IsDone()) {
				EndedDate = DateTime.UtcNow;
			}
		}

		public void AddTag(string tag) => Tags.Add(tag);
		public void RemoveTag(string tag) => Tags.Remove(tag);

		public void AddComment(Comment comment) => CommentIds.Add(comment.CommentId);
		public void AddComment(Guid commentId) => CommentIds.Add(commentId);
		public void RemoveComment(Comment comment) => CommentIds.Remove(comment.CommentId);
		public void RemoveComment(Guid commentId) => CommentIds.Remove(commentId);

		public void AddAssignee(User user) => AssigneeIds.Add(user.Id);
		public void AddAssignee(Guid userId) => AssigneeIds.Add(userId);
		public void RemoveAssignee(User user) => AssigneeIds.Remove(user.Id);
		public void RemoveAssignee(Guid userId) => AssigneeIds.Remove(userId);

		public void MarkAsInProgress() {
			if (!Status.IsCreated() && !Status.IsDone()) {
				throw new InvalidActionException($"Can't mark as in progress if you are in status {Status}");
			}
			Status = TaskStatus.IN_PROGRESS;
			StartedDate = DateTime.UtcNow;
			EndedDate = null;
		}

		public void MarkAsDone() {
			Status = TaskStatus.DONE;
			EndedDate = DateTime.UtcNow;
		}
	}
}
