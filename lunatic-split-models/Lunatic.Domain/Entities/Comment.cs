
using Lunatic.Domain.Utils;

namespace Lunatic.Domain.Entities {
    public class Comment : AuditableEntity {
        private Comment(Guid createdByUserId, Guid taskId, string content) : base(createdByUserId) {
            CommentId = Guid.NewGuid();
            TaskId = taskId;
            Content = content;
        }

        public Guid CommentId { get; private set; }
        public Guid TaskId { get; private set; }
        public string Content { get; private set; }

        public static Result<Comment> Create(Guid createdByUserId, Guid taskId, string content) {
            if(createdByUserId == default) {
                return Result<Comment>.Failure("Created by User Id is required.");
            }

            if(taskId == default) {
                return Result<Comment>.Failure("Task Id is required.");
            }

            if(string.IsNullOrWhiteSpace(content)) {
                return Result<Comment>.Failure("Content is required.");
            }

            return Result<Comment>.Success(new Comment(createdByUserId, taskId, content));
        }

        public void Update(string content) {
            Content = content;
            LastModifiedDate = DateTime.UtcNow;
        }

    }
}
