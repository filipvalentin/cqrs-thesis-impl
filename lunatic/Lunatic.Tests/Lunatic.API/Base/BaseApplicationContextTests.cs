
using Lunatic.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;


namespace Tests.Lunatic.API.Base {
	public abstract class BaseApplicationContextTests : IAsyncDisposable {
        protected readonly WebApplicationFactory<Program> Application;
        protected readonly HttpClient Client;

        protected BaseApplicationContextTests() {
            Application = new WebApplicationFactory<Program>();
            Application = Application.WithWebHostBuilder(builder => {
                builder.ConfigureServices(services => {
                    services.RemoveAll(typeof(DbContextOptions<LunaticContext>));
                    services.AddDbContext<LunaticContext>(options => {
                        options.UseInMemoryDatabase("LunaticDbForTesting");
                    });

                    services.Configure<JwtBearerOptions>(
                        JwtBearerDefaults.AuthenticationScheme,
                        options => {
                            options.Configuration = new OpenIdConnectConfiguration {
                                Issuer = JwtTokenProvider.Issuer,
                            };
                            options.TokenValidationParameters.ValidIssuer = JwtTokenProvider.Issuer;
                            options.TokenValidationParameters.ValidAudience = JwtTokenProvider.Issuer;
                            options.Configuration.SigningKeys.Add(JwtTokenProvider.SecurityKey);
                        }
                    );
                    var sp = services.BuildServiceProvider();
                    using (var scope = sp.CreateScope()) {
                        var scopedServices = scope.ServiceProvider;
                        var db = scopedServices.GetRequiredService<LunaticContext>();

                        db.Database.EnsureDeleted();
                        db.Database.EnsureCreated();

                        Seed.InitializeDbForTests(db);
                    }
                });
            });

            Client = Application.CreateClient();
        }

        public async ValueTask DisposeAsync() {
            GC.SuppressFinalize(this);
            await Application.DisposeAsync();
        }

        protected static string CreateToken() {
            return JwtTokenProvider.JwtSecurityTokenHandler.WriteToken(
            new JwtSecurityToken(
                JwtTokenProvider.Issuer,
                JwtTokenProvider.Issuer,
                new List<Claim> { new(ClaimTypes.Role, "User"), new("department", "Security") },
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: JwtTokenProvider.SigningCredentials
            ));
        }
    }
}
