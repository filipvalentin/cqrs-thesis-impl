using System.Text.Json;
using System.Text.Json.Serialization;

namespace Lunatic.UI.Services.Responses {
	public class ApiResponse<T> {
		public string Message { get; set; } = string.Empty;
		public bool Success { get; set; }
		public List<string>? ValidationErrors { get; set; }

		[JsonExtensionData]
		public IDictionary<string, JsonElement>? Data { get; set; }

		private readonly static JsonSerializerOptions options = new() { PropertyNameCaseInsensitive = true };
		public T GetValue(string propertyName) {
			if (Data is null) {
				throw new InvalidOperationException("Data is null");
			}

			return Data[propertyName].Deserialize<T>(options)!;
		}
		public string GetErrorsString() {
			return Message + " " + (ValidationErrors != null ? string.Join(", ", ValidationErrors) : "");
		}
		public override string ToString() {
			return $"ApiResponse(Message: {Message}, Success: {Success}, ValidationErrors: {string.Join(Environment.NewLine, ValidationErrors)}, Data: {string.Join(Environment.NewLine, Data)})";
		}
	}
	public class ApiResponse {
		public string Message { get; set; } = string.Empty;
		public bool Success { get; set; }
		public List<string>? ValidationErrors { get; set; }

		public string GetErrorsString() {
			return Message + " " + (ValidationErrors != null ? string.Join(", ", ValidationErrors) : "");
		}
	}
}
