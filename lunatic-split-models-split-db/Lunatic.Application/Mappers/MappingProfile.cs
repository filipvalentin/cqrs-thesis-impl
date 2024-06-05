using AutoMapper;
using Lunatic.Application.Features.Comments.Payload;
using Lunatic.Application.Features.Projects.Payload;
using Lunatic.Application.Features.Tasks.Payload;
using Lunatic.Application.Features.Teams.Payload;
using Lunatic.Application.Features.Users.Payload;
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
			CreateMap<Comment, CommentDto>().ForMember(t => t.AuthorId, opt => opt.MapFrom(src => src.CreatedByUserId));


			CreateMap<TeamReadModel, TeamDto>();
			CreateMap<TaskDescriptionReadModel, TaskDescriptionDto>()
				.ForMember(t => t.TaskId, opt => opt.MapFrom(src => src.Id));
			CreateMap<TaskReadModel, TaskDto>()
				.ForMember(t => t.TaskId, opt => opt.MapFrom(src => src.Id));
			CreateMap<TaskReadModel, FlatTaskDto>()
				.ForMember(t => t.TaskId, opt => opt.MapFrom(src => src.Id))
				.ForMember(t => t.Section, opt => opt.MapFrom(src => src.TaskSectionCard));
			CreateMap<ProjectReadModel, ProjectDto>()
				.ForMember(t => t.ProjectId, opt => opt.MapFrom(src => src.Id));
			CreateMap<CommentReadModel, CommentDto>()
				.ForMember(t => t.CommentId, opt => opt.MapFrom(src => src.Id));

		}
	}
}
