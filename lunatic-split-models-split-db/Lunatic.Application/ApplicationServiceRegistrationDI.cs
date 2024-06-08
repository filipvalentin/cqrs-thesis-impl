
using AutoMapper;
using FluentValidation;
using Lunatic.Application.Behaviors;
using Lunatic.Application.Mappers;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;


namespace Lunatic.Application {
	public static class ApplicationServiceRegistrationDI {
		public static IServiceCollection AddApplicationServices(this IServiceCollection services) {
			services.AddMediatR(
					cfg => cfg.RegisterServicesFromAssembly(
						Assembly.GetExecutingAssembly()));

			var mapperConfig = new MapperConfiguration(mc => {
				mc.AddProfile(new MappingProfile());
			});
			services.AddSingleton(mapperConfig.CreateMapper());

			services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));
			services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly(), includeInternalTypes: true);

			return services;
		}
	}
}

