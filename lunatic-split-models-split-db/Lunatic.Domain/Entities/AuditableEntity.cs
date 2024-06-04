
namespace Lunatic.Domain.Entities {
    public class AuditableEntity {
        public Guid CreatedByUserId { get; protected set; }
        public DateTime CreatedDate { get; protected set; }
        public Guid LastModifiedByUserId { get; set; }
        public DateTime LastModifiedDate { get; set; }

        protected AuditableEntity(Guid createdByUser) {
            CreatedByUserId = createdByUser;
            CreatedDate = DateTime.UtcNow;

            LastModifiedByUserId = CreatedByUserId;
            LastModifiedDate = CreatedDate;
        }
    }
}
