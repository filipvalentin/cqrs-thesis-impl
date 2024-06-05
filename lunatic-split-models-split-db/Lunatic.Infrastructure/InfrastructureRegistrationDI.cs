using Lunatic.Application.Contracts;
using Lunatic.Application.Features.Tasks.Interfaces;
using Lunatic.Application.Persistence.ReadSide;
using Lunatic.Application.Persistence.ReadSide.Task;
using Lunatic.Application.Persistence.WriteSide;
using Lunatic.Infrastructure.Data;
using Lunatic.Infrastructure.Providers;
using Lunatic.Infrastructure.ReadSideRepositories;
using Lunatic.Infrastructure.ReadSideRepositories.Comment;
using Lunatic.Infrastructure.ReadSideRepositories.Project;
using Lunatic.Infrastructure.ReadSideRepositories.Task;
using Lunatic.Infrastructure.ReadSideRepositories.Team;
using Lunatic.Infrastructure.ReadSideRepositories.User;
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

			services.AddSingleton<ILunaticReadContext>(sp => {
				var connectionString = configuration.GetConnectionString("MongoDb");
				var databaseName = configuration["MongoDb:DatabaseName"];
				return new LunaticReadContext(connectionString, databaseName);
			});

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

			services.AddScoped(typeof(IAsyncReadSideRepository<>), typeof(BaseReadSideRepository<>));
			services.AddScoped<ITeamReadSideRepository, TeamReadSideRepository>();
			services.AddScoped<ITaskReadSideRepository, TaskReadSideRepository>();
			services.AddScoped<IProjectReadSideRepository, ProjectReadSideRepository>();
			services.AddScoped<ICommentReadSideRepository, CommentReadSideRepository>();
			services.AddScoped<IUserReadSideRepository, UserReadSideRepository>();

			//services.AddScoped<ITeamReadService, TeamReadService>();
			//services.AddScoped<ITaskReadService, TaskReadService>();
			//services.AddScoped<IProjectReadService, ProjectReadService>();
			//services.AddScoped<ICommentReadService, CommentReadService>();

			services.AddScoped<IMLDataStorageService, MLDataStorageService>();
			services.AddScoped<IMLDataProvider, MLDataProvider>();

			return services;
		}
	}
}
