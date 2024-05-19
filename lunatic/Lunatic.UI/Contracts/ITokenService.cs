namespace Lunatic.UI.Contracts {
	public interface ITokenService {
		Task<string> GetTokenAsync();
		Task RemoveTokenAsync();
		Task SetTokenAsync(string token);
	}
}
