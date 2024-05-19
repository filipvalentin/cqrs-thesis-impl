
namespace Lunatic.Application.Features.Teams.Payload {
    public class TeamDto {
        public Guid TeamId { get; set; } = default!;
        public Guid OwnerId { get; set; } = default!;

        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;

        public List<Guid> MemberIds { get; set; } = default!;
        public List<Guid> ProjectIds { get; set; } = default!;
    }
}
