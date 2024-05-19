
using MediatR;


namespace Lunatic.Application.Features.Users.Commands.DeleteUser {
    public class DeleteUserCommand : IRequest<DeleteUserCommandResponse> {
        public Guid UserId { get; set; }
    }
}
