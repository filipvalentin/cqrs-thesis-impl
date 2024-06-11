using Lunatic.Application.Persistence.WriteSide;
using Lunatic.Domain.DomainEvents.User;
using MediatR;


namespace Lunatic.Application.Features.Users.Commands.UpdateUser {
	public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UpdateUserCommandResponse> {
		private readonly IUserRepository userRepository;
		private readonly IPublisher publisher;

		public UpdateUserCommandHandler(IUserRepository userRepository, IPublisher publisher) {
			this.userRepository = userRepository;
			this.publisher = publisher;
		}

		public async Task<UpdateUserCommandResponse> Handle(UpdateUserCommand request, CancellationToken cancellationToken) {
			//var validator = new UpdateUserCommandValidator(userRepository);
			//var validatorResult = await validator.ValidateAsync(request, cancellationToken);

			//if (!validatorResult.IsValid) {
			//	return new UpdateUserCommandResponse {
			//		Success = false,
			//		ValidationErrors = validatorResult.Errors.Select(e => e.ErrorMessage).ToList()
			//	};
			//}

			var userResult = await userRepository.FindByIdAsync(request.UserId);
			if (!userResult.IsSuccess) {
				return new UpdateUserCommandResponse {
					Success = false,
					ValidationErrors = new List<string> { "User not found" }
				};
			}

			userResult.Value.Update(request.FirstName, request.LastName, request.Email);

			var dbUserResult = await userRepository.UpdateAsync(userResult.Value);

			await publisher.Publish(new UserUpdatedDomainEvent(userResult.Value.Id), cancellationToken);

			return new UpdateUserCommandResponse {
				Success = true
			};
		}
	}
}
