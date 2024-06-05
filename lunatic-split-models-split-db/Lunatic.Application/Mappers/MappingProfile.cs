using AutoMapper;
using Lunatic.Application.Features.Comments.Payload;
using Lunatic.Application.Models.ReadModels;
using Lunatic.Application.Models.ReadModels.Tasks;
using Lunatic.Domain.Entities;

namespace Lunatic.Application.Mappers {
	public class MappingProfile : Profile {
		public MappingProfile() {
			CreateMap<Team, TeamReadModel>();
			CreateMap<Project, ProjectReadModel>();
			CreateMap<Comment, CommentReadModel>();
			CreateMap<Domain.Entities.Task, TaskReadModel>();
			CreateMap<User, UserReadModel>();
			CreateMap<User, UserDto>();

			CreateMap<Comment, CommentDto>().ForMember(t=>t.AuthorId, opt => opt.MapFrom(src => src.CreatedByUserId));
		}
	}
}
