
using Lunatic.Domain.Entities;
using MediatR;


namespace Lunatic.Application.Features.Users.Commands.CreateUser {
    public class CreateUserCommand : IRequest<CreateUserCommandResponse> {
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Username { get; set; } = default!;
        public string Password { get; set; } = default!;
        public Role Role { get; set; } = default!;
    }
}
