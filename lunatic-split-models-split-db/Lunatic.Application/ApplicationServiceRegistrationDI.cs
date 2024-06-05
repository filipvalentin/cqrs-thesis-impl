
using AutoMapper;
using Lunatic.Application.Mappers;
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

			return services;
		}
	}
}

