using Lunatic.Application.Features.Users.Payload;
using MediatR;
using Lunatic.Application.Persistence.ReadSide;
using AutoMapper;


namespace Lunatic.Application.Features.Users.Queries.GetById {
	public class GetByIdUserQueryHandler : IRequestHandler<GetByIdUserQuery, GetByIdUserQueryResponse> {
        private readonly IUserReadSideRepository userRepository;
		private readonly IMapper mapper;
		public GetByIdUserQueryHandler(IUserReadSideRepository userRepository, IMapper mapper) {
			this.userRepository = userRepository;
			this.mapper = mapper;
		}

		public async Task<GetByIdUserQueryResponse> Handle(GetByIdUserQuery request, CancellationToken cancellationToken) {
            var userResult = await userRepository.FindByIdAsync(request.UserId);
            if(!userResult.IsSuccess) {
                return new GetByIdUserQueryResponse {
                    Success = false,
                    Message = userResult.Error
                };
            }

			var user = userResult.Value;

			return new GetByIdUserQueryResponse {
				Success = true,
				User = mapper.Map<UserDto>(user)
			};
        }
    }
}
