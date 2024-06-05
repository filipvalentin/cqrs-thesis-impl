using Lunatic.Application.Persistence.WriteSide;
using Lunatic.Domain.DomainEvents.User;
using MediatR;


namespace Lunatic.Application.Features.Users.Commands.DeleteUser
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, DeleteUserCommandResponse> {
        private readonly IUserRepository userRepository;
		private readonly IPublisher publisher;

		public DeleteUserCommandHandler(IUserRepository userRepository, IPublisher publisher) {
			this.userRepository = userRepository;
			this.publisher = publisher;
		}

		public async Task<DeleteUserCommandResponse> Handle(DeleteUserCommand request, CancellationToken cancellationToken) {
            var result = await userRepository.DeleteAsync(request.UserId);

            if(!result.IsSuccess) {
                return new DeleteUserCommandResponse {
                    Success = false,
                    ValidationErrors = new List<string> { result.Error }
                };
            }

			await publisher.Publish(new UserDeletedDomainEvent(request.UserId), cancellationToken);

            return new DeleteUserCommandResponse {
                Success = true
            };
        }
    }
}
