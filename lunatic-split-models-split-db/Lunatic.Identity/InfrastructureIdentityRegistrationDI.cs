using Lunatic.Application.Contracts.Identity;
using Lunatic.Identity.Models;
using Lunatic.Identity.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;


namespace Lunatic.Identity {
	public static class InfrastructureIdentityRegistrationDI {
		public static IServiceCollection AddInfrastructureIdentityToDI(
					   this IServiceCollection services,
								  IConfiguration configuration) {
			services.AddDbContext<LunaticUserContext>(
			   options => options.UseNpgsql(
				   configuration.GetConnectionString("LunaticUserConnection"),
				   builder => builder.MigrationsAssembly(typeof(LunaticUserContext).Assembly.FullName)));

			services.AddIdentity<ApplicationUser, IdentityRole>()
							.AddEntityFrameworkStores<LunaticUserContext>()
							.AddDefaultTokenProviders();

			services.AddAuthentication(options => {
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
			})
				.AddJwtBearer(options => {
					options.SaveToken = true;
					options.RequireHttpsMetadata = false;
					options.TokenValidationParameters = new TokenValidationParameters() {
						ValidateIssuer = true,
						ValidateAudience = true,
						ValidAudience = configuration["JWT:ValidAudience"],
						ValidIssuer = configuration["JWT:ValidIssuer"],
						ClockSkew = TimeSpan.Zero,
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]!))
					};
				});

			services.AddScoped<IAuthService, AuthService>();

			return services;
		}

		public static IServiceCollection AddLazyResolution(this IServiceCollection services) {
			return services.AddTransient(
				typeof(Lazy<>),
				typeof(LazilyResolved<>));
		}

		private class LazilyResolved<T> : Lazy<T> {
			public LazilyResolved(IServiceProvider serviceProvider)
				: base(serviceProvider.GetRequiredService<T>) {
			}
		}
	}
}
