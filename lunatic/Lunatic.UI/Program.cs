using Blazored.LocalStorage;
using GlobalBuyTicket.App.Services;
using Lunatic.UI.Auth;
using Lunatic.UI.Contracts;
using Lunatic.UI.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Lunatic.UI {
	public class Program {
		public static async Task Main(string[] args) {
			var builder = WebAssemblyHostBuilder.CreateDefault(args);
			builder.RootComponents.Add<App>("#app");
			builder.RootComponents.Add<HeadOutlet>("head::after");


			builder.Services.AddAuthorizationCore();
			builder.Services.AddBlazoredLocalStorage(config => {
				config.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
				config.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
				config.JsonSerializerOptions.IgnoreReadOnlyProperties = true;
				config.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
				config.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
				config.JsonSerializerOptions.ReadCommentHandling = JsonCommentHandling.Skip;
				config.JsonSerializerOptions.WriteIndented = false;
			});
			builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });


			builder.Services.AddScoped<ITokenService, TokenService>();
			builder.Services.AddScoped<CustomStateProvider>();
			builder.Services.AddHttpClient<ITeamDataService, TeamDataService>(client => {
				client.BaseAddress = new Uri("https://localhost:7555/");
			});
			builder.Services.AddHttpClient<IProjectDataService, ProjectDataService>(client => {
				client.BaseAddress = new Uri("https://localhost:7555/");
			});
			builder.Services.AddScoped<AuthenticationStateProvider>(s => s.GetRequiredService<CustomStateProvider>());
			builder.Services.AddHttpClient<IAuthenticationService, AuthenticationService>(client => {
				client.BaseAddress = new Uri("https://localhost:7555/");
			});
			builder.Services.AddHttpClient<IUserDataService, UserDataService>(client => {
				client.BaseAddress = new Uri("https://localhost:7555/");
			});
			builder.Services.AddHttpClient<ITaskDataService, TaskDataService>(client => {
				client.BaseAddress = new Uri("https://localhost:7555/");
			});
			builder.Services.AddHttpClient<ICommentDataService, CommentDataService>(client => {
				client.BaseAddress = new Uri("https://localhost:7555/");
			});
			builder.Services.AddHttpClient<IImageService, ImageService>(client => {
				client.BaseAddress = new Uri("https://localhost:7555/");
			});

			builder.Services.AddMudServices(config => {
				//config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomLeft;

				//config.SnackbarConfiguration.PreventDuplicates = false;
				//config.SnackbarConfiguration.NewestOnTop = false;
				//config.SnackbarConfiguration.ShowCloseIcon = true;
				config.SnackbarConfiguration.VisibleStateDuration = 5000;
				config.SnackbarConfiguration.HideTransitionDuration = 100;
				config.SnackbarConfiguration.ShowTransitionDuration = 500;
				//config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
			});

			await builder.Build().RunAsync();

		}
	}
}
