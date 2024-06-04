using Lunatic.Application.Persistence.WriteSide;
using MediatR;


namespace Lunatic.Application.Features.Users.Commands.DeleteUser
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, DeleteUserCommandResponse> {
        private readonly IUserRepository userRepository;

        public DeleteUserCommandHandler(IUserRepository userRepository) {
            this.userRepository = userRepository;
        }

        public async Task<DeleteUserCommandResponse> Handle(DeleteUserCommand request, CancellationToken cancellationToken) {
            var result = await userRepository.DeleteAsync(request.UserId);

            if(!result.IsSuccess) {
                return new DeleteUserCommandResponse {
                    Success = false,
                    ValidationErrors = new List<string> { result.Error }
                };

            }
            return new DeleteUserCommandResponse {
                Success = true
            };
        }
    }
}
