using Lunatic.Application.Features.Tasks.Interfaces;
using Lunatic.Application.Models;
using Lunatic.Domain.MLModel;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;

namespace Lunatic.Infrastructure.Providers {
	internal class MLDataProvider : IMLDataProvider {
		private readonly IOptions<MLAPISettings> options;
		public string Url { get; init; }
		public MLDataProvider(IOptions<MLAPISettings> options) {
			this.options = options;
			Url = $"{options.Value.MLAPIAddress}/predict";
		}

		public async Task<int> GetTaskPrediction(DaysToCompleteTaskEntry payload) {
			var httpClient = new HttpClient();

			var jsonPayload = JsonSerializer.Serialize(payload);
			var httpContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

			var response = await httpClient.PostAsync(Url, httpContent);

			if (response.IsSuccessStatusCode) {
				var prediction = await response.Content.ReadAsStringAsync();
				return int.Parse(prediction);
			}

			throw new Exception(response.Content.ToString());
		}
	}
}
