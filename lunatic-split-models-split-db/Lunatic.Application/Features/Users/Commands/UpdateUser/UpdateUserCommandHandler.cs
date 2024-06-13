using AutoMapper;
using Lunatic.Application.Persistence.WriteSide;
using Lunatic.Domain.DomainEvents.User;
using MediatR;

namespace Lunatic.Application.Features.Users.Commands.UpdateUser {
	public class UpdateUserCommandHandler(
		IUserRepository userRepository,
		IPublisher publisher,
		IMapper mapper,
		IUnitOfWork unitOfWork) : IRequestHandler<UpdateUserCommand, UpdateUserCommandResponse> {

		private readonly IUserRepository userRepository = userRepository;
		private readonly IPublisher publisher = publisher;
		private readonly IMapper mapper = mapper;
		private readonly IUnitOfWork unitOfWork = unitOfWork;

		public async Task<UpdateUserCommandResponse> Handle(UpdateUserCommand request, CancellationToken cancellationToken) {

			var userResult = await userRepository.FindByIdAsync(request.UserId);
			if (!userResult.IsSuccess) {
				return new UpdateUserCommandResponse {
					Success = false,
					Message = userResult.Error
				};
			}
			var user = userResult.Value;
			user.Update(request.FirstName, request.LastName, request.Email);

			var dbUserResult = await userRepository.UpdateAsync(user);
			if (!dbUserResult.IsSuccess) {
				return new UpdateUserCommandResponse {
					Success = false,
					Message = dbUserResult.Error
				};
			}

			await unitOfWork.SaveChangesAsync(cancellationToken);

			await publisher.Publish(mapper.Map<UserUpdatedDomainEvent>(user), cancellationToken);

			return new UpdateUserCommandResponse {
				Success = true
			};
		}
	}
}
