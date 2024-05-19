
using Lunatic.Application.Contracts;
using Lunatic.Application.Persistence;
using Lunatic.Infrastructure.Repositories;
using Lunatic.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Lunatic.Infrastructure {
	public static class InfrastructureRegistrationDI {
		public static IServiceCollection AddInfrastructureToDI(
			this IServiceCollection services,
			IConfiguration configuration) {
			services.AddDbContext<LunaticContext>(
				options => options.UseNpgsql(
						configuration.GetConnectionString("LunaticConnection"),
						builder => builder.MigrationsAssembly(typeof(LunaticContext).Assembly.FullName)));

			services.AddScoped(typeof(IAsyncRepository<>), typeof(BaseRepository<>));
			services.AddScoped<ICommentRepository, CommentRepository>();
			services.AddScoped<IProjectRepository, ProjectRepository>();
			services.AddScoped<ITaskRepository, TaskRepository>();
			services.AddScoped<ITeamRepository, TeamRepository>();
			services.AddScoped<IUserRepository, UserRepository>();
			services.AddScoped<IImageRepository, ImageRepository>();
			services.AddScoped<IEmailService, EmailService>();
			return services;
		}
	}
}
