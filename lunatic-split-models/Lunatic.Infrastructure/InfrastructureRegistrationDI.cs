using Lunatic.Application.Contracts;
using Lunatic.Application.Contracts.Interfaces;
using Lunatic.Application.Features.Comments;
using Lunatic.Application.Features.Projects;
using Lunatic.Application.Features.Tasks.Interfaces;
using Lunatic.Application.Features.Teams;
using Lunatic.Application.Persistence;
using Lunatic.Infrastructure.Data;
using Lunatic.Infrastructure.Providers;
using Lunatic.Infrastructure.ReadServices;
using Lunatic.Infrastructure.Repositories;
using Lunatic.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Lunatic.Infrastructure
{
    public static class InfrastructureRegistrationDI {
		public static IServiceCollection AddInfrastructureToDI(this IServiceCollection services, IConfiguration configuration) {

			services.AddDbContext<LunaticContext>(
				options => options.UseNpgsql(
						configuration.GetConnectionString("LunaticConnection"),
						builder => builder.MigrationsAssembly(typeof(LunaticContext).Assembly.FullName)));

			services.AddDbContext<LunaticReadContext>(
				options => options.UseNpgsql(
						configuration.GetConnectionString("LunaticReadConnection"),
						builder => builder.MigrationsAssembly(typeof(LunaticReadContext).Assembly.FullName))
				.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
				);

			services.AddDbContext<LunaticMLContext>(
				options => options.UseSqlServer(
						configuration.GetConnectionString("LunaticMLConnection"),
						builder => builder.MigrationsAssembly(typeof(LunaticMLContext).Assembly.FullName))
				.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
				);

			services.AddScoped(typeof(IAsyncRepository<>), typeof(BaseRepository<>));
			services.AddScoped<ICommentRepository, CommentRepository>();
			services.AddScoped<IProjectRepository, ProjectRepository>();
			services.AddScoped<ITaskRepository, TaskRepository>();
			services.AddScoped<ITeamRepository, TeamRepository>();
			services.AddScoped<IUserRepository, UserRepository>();
			services.AddScoped<IImageRepository, ImageRepository>();
			services.AddScoped<IEmailService, EmailService>();

			services.AddScoped(typeof(IGenericReadService<>), typeof(GenericReadService<>));
			services.AddScoped<ITeamReadService, TeamReadService>();
			services.AddScoped<ITaskReadService, TaskReadService>();
			services.AddScoped<IProjectReadService, ProjectReadService>();
			services.AddScoped<ICommentReadService, CommentReadService>();
			services.AddScoped<IMLDataStorageService, MLDataStorageService>();
			services.AddScoped<IMLDataProvider, MLDataProvider>();

			return services;
		}
	}
}
