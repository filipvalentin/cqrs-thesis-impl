using Lunatic.Domain.Entities;
using Lunatic.Application.Features.Users.Payload;
using MediatR;
using Lunatic.Application.Persistence.WriteSide;
using Lunatic.Domain.DomainEvents.User;
using AutoMapper;


namespace Lunatic.Application.Features.Users.Commands.CreateUser {
	public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, CreateUserCommandResponse> {
		private readonly IUserRepository userRepository;
		private readonly IPublisher publisher;
		private readonly IMapper mapper;

		public CreateUserCommandHandler(IUserRepository userRepository, IPublisher publisher, IMapper mapper) {
			this.userRepository = userRepository;
			this.publisher = publisher;
			this.mapper = mapper;
		}

		public async Task<CreateUserCommandResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken) {
			var validator = new CreateUserCommandValidator(userRepository);
			var validatorResult = await validator.ValidateAsync(request, cancellationToken);

			if (!validatorResult.IsValid) {
				return new CreateUserCommandResponse {
					Success = false,
					ValidationErrors = validatorResult.Errors.Select(e => e.ErrorMessage).ToList()
				};
			}

			var user = User.Create(request.FirstName, request.LastName, request.Email, request.Username, request.Password, request.Role).Value;

			await userRepository.AddAsync(user);

			await publisher.Publish(new UserCreatedDomainEvent(user.Id));

			return new CreateUserCommandResponse {
				Success = true,
				User = mapper.Map<UserDto>(user)
			};
		}
	}
}
