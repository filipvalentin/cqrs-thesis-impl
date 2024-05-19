namespace Lunatic.UI.Models.Dtos
{
    public class AddTeamMemberDto
    {
        public Guid UserId { get; set; } = default!;
        public Guid TeamId { get; set; } = default!;
    }
}
