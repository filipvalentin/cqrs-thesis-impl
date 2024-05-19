using MediatR;


namespace Lunatic.Application.Features.Users.Commands.UpdateUser {
	public class UpdateUserCommand : IRequest<UpdateUserCommandResponse> {
        public Guid UserId { get; set; } = default!;
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string Email { get; set; } = default!;
        //public string Username { get; set; } = default!;
    }
}
