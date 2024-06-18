using Lunatic.Application.Contracts;
using Lunatic.Application.Features.Tasks.Interfaces;
using Lunatic.Application.Persistence.ReadSide;
using Lunatic.Application.Persistence.ReadSide.Task;
using Lunatic.Application.Persistence.WriteSide;
using Lunatic.Application.Utils.Services;
using Lunatic.Domain.Primitives;
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
using Microsoft.Extensions.Hosting;

namespace Lunatic.Infrastructure {
	public static class InfrastructureRegistrationDI {
		public static IServiceCollection AddInfrastructureToDI(this IServiceCollection services, IConfiguration configuration) {

			services.AddDbContext<LunaticContext>(
				options => options.UseNpgsql(
						configuration.GetConnectionString("LunaticConnection"),
						builder => builder.MigrationsAssembly(typeof(LunaticContext).Assembly.FullName)));

			services.AddSingleton<ILunaticReadContext>(sp => {
				var connectionString = configuration.GetConnectionString("MongoDb");
				var databaseName = configuration["MongoDb:DatabaseName"];
				return new LunaticReadContext(connectionString!, databaseName!);
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

			services.AddTransient(typeof(IAsyncReadSideRepository<>), typeof(BaseReadSideRepository<>));
			services.AddTransient<ITeamReadSideRepository, TeamReadSideRepository>();
			services.AddTransient<ITaskReadSideRepository, TaskReadSideRepository>();
			services.AddTransient<IProjectReadSideRepository, ProjectReadSideRepository>();
			services.AddTransient<ICommentReadSideRepository, CommentReadSideRepository>();
			services.AddTransient<IUserReadSideRepository, UserReadSideRepository>();

			services.AddScoped<IMLDataStorageService, MLDataStorageService>();
			services.AddScoped<IMLDataProvider, MLDataProvider>();

			services.AddSingleton<IEventQueueService>(provider => new RabbitMqEventQueueService(configuration["RabbitMQ:Host"]!));
			//services.AddFailedEventProcessorServices();

			services.AddScoped<IUnitOfWork, UnitOfWork>();

			return services;
		}
	}

	public static class ServiceCollectionExtensions {
		public static IServiceCollection AddFailedEventProcessorServices(this IServiceCollection services) {
			var domainEventTypes = AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany(a => a.GetTypes())
				.Where(t => typeof(IDomainEvent).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

			foreach (var domainEventType in domainEventTypes) {
				var serviceType = typeof(FailedEventProcessorService<>).MakeGenericType(domainEventType);
				services.AddSingleton(typeof(IHostedService), serviceType);
			}

			return services;
		}
	}
}
