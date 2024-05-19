
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;


namespace Lunatic.Application {
	public static class ApplicationServiceRegistrationDI {
		public static IServiceCollection AddApplicationServices(this IServiceCollection services) {
			services.AddMediatR(
					cfg => cfg.RegisterServicesFromAssembly(
						Assembly.GetExecutingAssembly()));
			return services;
		}
	}
}

