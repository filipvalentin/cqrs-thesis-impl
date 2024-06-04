
using Lunatic.Domain.Entities;


namespace Tests.Lunatic.Domain.Entities.UserTests {
	public class CreateTests {
		public const string FirstName = "John";
		public const string LastName = "Smith";
		public const string Email = "smith@gmail.com";
		public const string Username = "smith";
		public const string Password = "String123#";
		public const Role MockRole = Role.USER;

		[Fact]
		public void GivenValidUser_WhenCreateUser_ThenSuccessResult() {
			// given

			// when
			var userResult = User.Create(FirstName, LastName, Email, Username, Password, MockRole);

			// then
			Assert.True(userResult.IsSuccess);
		}

		[Theory]
		[InlineData(null, LastName, Email, Username, Password, MockRole)]
		[InlineData(FirstName, null, Email, Username, Password, MockRole)]
		[InlineData(FirstName, LastName, null, Username, Password, MockRole)]
		[InlineData(FirstName, LastName, Email, null, Password, MockRole)]
		[InlineData(FirstName, LastName, Email, Username, null, MockRole)]
		[InlineData("", LastName, Email, Username, Password, MockRole)]
		[InlineData(FirstName, "", Email, Username, Password, MockRole)]
		[InlineData(FirstName, LastName, "", Username, Password, MockRole)]
		[InlineData(FirstName, LastName, Email, "", Password, MockRole)]
		[InlineData(FirstName, LastName, Email, Username, "", MockRole)]
		public void GivenInvalidUser_WhenCreateUser_ThenFailureResult(
				string firstName, string lastName, string email, string username, string password, Role role) {
			// given

			// when
			var userResult = User.Create(firstName, lastName, email, username, password, role);

			// then
			Assert.False(userResult.IsSuccess);
		}
	}
}

