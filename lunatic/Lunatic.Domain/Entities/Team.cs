
using Lunatic.Domain.Utils;

namespace Lunatic.Domain.Entities {
	public class Team : AuditableEntity {
		private Team(Guid createdByUserId, string name, string description) : base(createdByUserId) {
			Id = Guid.NewGuid();
			Name = name;
			Description = description;
		}

		public Guid Id { get; private set; }
		public string Name { get; private set; }
		public string Description { get; private set; }
		public List<Guid> MemberIds { get; private set; } = new List<Guid>();
		public List<Guid> ProjectIds { get; private set; } = new List<Guid>();

		public static Result<Team> Create(Guid createdByUserId, string name, string description) {
			if (createdByUserId == default) {
				return Result<Team>.Failure("Created by User Id is required.");
			}

			if (string.IsNullOrWhiteSpace(name)) {
				return Result<Team>.Failure("Name is required.");
			}

			if (string.IsNullOrWhiteSpace(description)) {
				return Result<Team>.Failure("Description is required.");
			}

			return Result<Team>.Success(new Team(createdByUserId, name, description));
		}

		public void Update(string name, string description) {
			Name = name;
			Description = description;
			LastModifiedDate = DateTime.UtcNow;
		}

		//public void AddMember(User user) => MemberIds.Add(user.Id);
		public void AddMember(Guid userId) => MemberIds.Add(userId);
		//public void RemoveMember(User user) => MemberIds.Remove(user.Id);
		public void RemoveMember(Guid userId) => MemberIds.Remove(userId);

		//public void AddProject(Project project) => ProjectIds.Add(project.Id);
		public void AddProject(Guid projectId) => ProjectIds.Add(projectId);
		//public void RemoveProject(Project project) => ProjectIds.Remove(project.Id);
		public void RemoveProject(Guid projectId) => ProjectIds.Remove(projectId);
	}
}
