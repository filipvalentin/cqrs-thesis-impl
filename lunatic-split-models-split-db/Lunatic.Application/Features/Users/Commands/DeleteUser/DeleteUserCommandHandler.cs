using Lunatic.Application.Contracts.Interfaces;
using Lunatic.Application.Persistence.WriteSide;
using Lunatic.Domain.DomainEvents.Team;
using Lunatic.Domain.DomainEvents.User;
using MediatR;


namespace Lunatic.Application.Features.Users.Commands.DeleteUser {
	public class DeleteUserCommandHandler(
		IUserRepository userRepository,
		IPublisher publisher,
		ITeamRepository teamRepository,
		IImageService imageService,
		IUnitOfWork unitOfWork) : IRequestHandler<DeleteUserCommand, DeleteUserCommandResponse> {

		private readonly IUserRepository userRepository = userRepository;
		private readonly IImageService imageService = imageService;
		private readonly ITeamRepository teamRepository = teamRepository;
		private readonly IPublisher publisher = publisher;
		private readonly IUnitOfWork unitOfWork = unitOfWork;

		public async Task<DeleteUserCommandResponse> Handle(DeleteUserCommand request, CancellationToken cancellationToken) {

			var deletedUserResult = await userRepository.DeleteAsync(request.UserId);
			if (!deletedUserResult.IsSuccess) {
				return new DeleteUserCommandResponse {
					Success = false,
					Message = deletedUserResult.Error
				};
			}
			var deletedUser = deletedUserResult.Value;

			foreach (var teamId in deletedUser.TeamIds) {
				var deletedTeamResult = await teamRepository.DeleteAsync(teamId);
				if (!deletedTeamResult.IsSuccess) {
					return new DeleteUserCommandResponse {
						Success = false,
						Message = deletedTeamResult.Error
					};
				}
				await publisher.Publish(new TeamDisbandedDomainEvent(teamId, deletedTeamResult.Value.ProjectIds), cancellationToken);
			}

			await imageService.DeleteUserImage(request.UserId);

			await unitOfWork.SaveChangesAsync(cancellationToken);

			await publisher.Publish(new UserDeletedDomainEvent(request.UserId), cancellationToken);

			return new DeleteUserCommandResponse {
				Success = true
			};
		}
	}
}
