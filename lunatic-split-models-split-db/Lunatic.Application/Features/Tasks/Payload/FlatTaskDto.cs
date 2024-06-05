
using Lunatic.Application.Models.ReadModels;
using Lunatic.Domain.Entities;
using TaskStatus = Lunatic.Domain.Entities.TaskStatus;


namespace Lunatic.Application.Features.Tasks.Payload {
    public class FlatTaskDto {
        public Guid CreatedByUserId { get; set; } = default!;
        public Guid TaskId { get; set; } = default!;
        public Guid ProjectId { get; set; } = default!;
        public string Section { get; set; } = default!;
        public string Title { get; set; } = default!;
        public string Description { get; set; } = default!;
        public TaskPriority Priority { get; set; } = default!;
        public TaskStatus Status { get; set; } = default!;
        public List<string> Tags { get; set; } = default!;
        public List<CommentReadModel> Comments { get; set; } = default!;
        public List<Guid> AssigneeIds { get; set; } = default!;
        public DateTime PlannedStartDate { get; set; } = default!;
        public DateTime PlannedEndDate { get; set; } = default!;
        public DateTime? StartedDate { get; set; } = default!;
        public DateTime? EndedDate { get; set; } = default!;
    }
}
