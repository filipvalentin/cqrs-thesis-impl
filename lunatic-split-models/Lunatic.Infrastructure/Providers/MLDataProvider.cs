using Lunatic.Application.Features.Tasks.Interfaces;
using Lunatic.Application.Models;
using Lunatic.Domain.MLModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

			var jsonPayload = JsonConvert.SerializeObject(payload);
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
