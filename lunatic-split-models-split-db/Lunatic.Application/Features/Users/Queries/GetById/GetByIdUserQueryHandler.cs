using Lunatic.Application.Features.Users.Payload;
using MediatR;
using Lunatic.Application.Persistence.WriteSide;


namespace Lunatic.Application.Features.Users.Queries.GetById
{
    public class GetByIdUserQueryHandler : IRequestHandler<GetByIdUserQuery, GetByIdUserQueryResponse> {
        private readonly IUserRepository userRepository;

        public GetByIdUserQueryHandler(IUserRepository userRepository) {
            this.userRepository = userRepository;
        }

        public async Task<GetByIdUserQueryResponse> Handle(GetByIdUserQuery request, CancellationToken cancellationToken) {
            var userResult = await userRepository.FindByIdAsync(request.UserId);
            if(!userResult.IsSuccess) {
                return new GetByIdUserQueryResponse {
                    Success = false,
                    ValidationErrors = new List<string> { "User not found" }
                };
            }

            return new GetByIdUserQueryResponse { 
                Success = true,
                User = new UserDto {
                    UserId = userResult.Value.Id,

                    FirstName = userResult.Value.FirstName,
                    LastName = userResult.Value.LastName,
                    Email = userResult.Value.Email,
                    Username = userResult.Value.Username,
                    Role = userResult.Value.Role,

                    TeamIds = userResult.Value.TeamIds
                }
            };
        }
    }
}
