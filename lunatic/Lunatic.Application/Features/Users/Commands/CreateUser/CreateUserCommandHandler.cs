
using Lunatic.Application.Persistence;
using Lunatic.Domain.Entities;
using Lunatic.Application.Features.Users.Payload;
using MediatR;


namespace Lunatic.Application.Features.Users.Commands.CreateUser {
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, CreateUserCommandResponse> {
        private readonly IUserRepository userRepository;

        public CreateUserCommandHandler(IUserRepository userRepository) {
            this.userRepository = userRepository;
        }

        public async Task<CreateUserCommandResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken) {
            var validator = new CreateUserCommandValidator(userRepository);
            var validatorResult = await validator.ValidateAsync(request, cancellationToken);

            if(!validatorResult.IsValid) {
                return new CreateUserCommandResponse {
                    Success = false,
                    ValidationErrors = validatorResult.Errors.Select(e => e.ErrorMessage).ToList()
                };
            }

            var user = User.Create(request.FirstName, request.LastName, request.Email, request.Username, request.Password, request.Role).Value;

            await userRepository.AddAsync(user);

            return new CreateUserCommandResponse {
                Success = true,
                User = new UserDto {
                    UserId = user.Id,

                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Username = user.Username,
                    Role = user.Role,

                    TeamIds = user.TeamIds
                }
            };
        }
    }
}
