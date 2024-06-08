﻿using Lunatic.Application.Features.Teams.Payload;
using MediatR;
using Lunatic.Application.Persistence.WriteSide;
using AutoMapper;

namespace Lunatic.Application.Features.Teams.Commands.AddTeamMember {
	public class AddTeamMemberCommandHandler : IRequestHandler<AddTeamMemberCommand, AddTeamMemberCommandResponse> {
		private readonly ITeamRepository teamRepository;
		private readonly IUserRepository userRepository;
		private readonly IMapper mapper;

		public AddTeamMemberCommandHandler(ITeamRepository teamRepository, IUserRepository userRepository, IMapper mapper) {
			this.teamRepository = teamRepository;
			this.userRepository = userRepository;
			this.mapper = mapper;
		}

		public async Task<AddTeamMemberCommandResponse> Handle(AddTeamMemberCommand request, CancellationToken cancellationToken) {
			var validator = new AddTeamMemberCommandValidator(userRepository, teamRepository);
			var validatorResult = await validator.ValidateAsync(request, cancellationToken);

			if (!validatorResult.IsValid) {
				return new AddTeamMemberCommandResponse {
					Success = false,
					ValidationErrors = validatorResult.Errors.Select(e => e.ErrorMessage).ToList()
				};
			}

			var team = (await teamRepository.FindByIdAsync(request.TeamId)).Value;
			team.AddMember(request.UserId);

			var user = (await userRepository.FindByIdAsync(request.UserId)).Value;
			user.AddTeam(request.TeamId);
			await userRepository.UpdateAsync(user);

			var dbTeamResult = await teamRepository.UpdateAsync(team);

			return new AddTeamMemberCommandResponse {
				Success = true,
				Team = mapper.Map<TeamDto>(dbTeamResult.Value)
			};
		}
	}
}
