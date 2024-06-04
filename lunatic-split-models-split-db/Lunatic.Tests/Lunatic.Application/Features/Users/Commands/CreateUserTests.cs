
using Lunatic.Application.Features.Users.Commands.CreateUser;
using Lunatic.Application.Persistence;
using Lunatic.Domain.Entities;
using NSubstitute;


namespace Tests.Lunatic.Application.Features.Users.Commands {
	public class CreateUserTests {
		public const string FirstName = "John";
		public const string LastName = "Smith";
		public const string Email = "smith@gmail.com";
		public const string Username = "smith";
		public const string Password = "String123#";
		public const Role MockRole = Role.USER;

		[Fact]
		public async void GivenCreateUserComamnd_WhenCreate_ThenSuccessResponse() {
			// given
			var userRepository = Substitute.For<IUserRepository>();
			var command = new CreateUserCommand {
				FirstName = FirstName,
				LastName = LastName,
				Email = Email,
				Username = Username,
				Password = Password,
				Role = MockRole
			};
			var commandHandler = new CreateUserCommandHandler(userRepository);
			var source = new CancellationTokenSource();

			// when
			var response = await commandHandler.Handle(command, source.Token);

			// then
			Assert.True(response.Success);
			Assert.NotNull(response.User);
			Assert.Null(response.ValidationErrors);
		}

		[Theory]
		[InlineData(null, LastName, Email, Username, Password, MockRole)]
		[InlineData("", LastName, Email, Username, Password, MockRole)]
		[InlineData(FirstName, null, Email, Username, Password, MockRole)]
		[InlineData(FirstName, "", Email, Username, Password, MockRole)]
		[InlineData(FirstName, LastName, null, Username, Password, MockRole)]
		[InlineData(FirstName, LastName, "", Username, Password, MockRole)]
		[InlineData(FirstName, LastName, Email, null, Password, MockRole)]
		[InlineData(FirstName, LastName, Email, "", Password, MockRole)]
		[InlineData(FirstName, LastName, Email, Username, null, MockRole)]
		[InlineData(FirstName, LastName, Email, Username, "", MockRole)]
		public async void GivenInvalidCreateUserComamnd_WhenCreate_ThenFailureResponse(
				string firstName, string lastName, string email, string username, string password, Role role) {
			// given
			var userRepository = Substitute.For<IUserRepository>();
			var command = new CreateUserCommand {
				FirstName = firstName,
				LastName = lastName,
				Email = email,
				Username = username,
				Password = password,
				Role = role
			};
			var commandHandler = new CreateUserCommandHandler(userRepository);
			var source = new CancellationTokenSource();

			// when
			var response = await commandHandler.Handle(command, source.Token);

			// then
			Assert.False(response.Success);
			Assert.Null(response.User);
			Assert.NotNull(response.ValidationErrors);
		}
	}
}

