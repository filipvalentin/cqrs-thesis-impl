
using Lunatic.Domain.Entities;


namespace Tests.Lunatic.Domain.Entities.UserTests {
    public class UpdateTests {
		public const string FirstName = "John";
		public const string LastName = "Smith";
		public const string Email = "smith@gmail.com";
		public const string Username = "smith";
		public const string Password = "String123#";

		[Fact]
        public void GivenUser_WhenUpdateUser_ThenUpdatedSuccessfully() {
            // given
            var user = User.Create(FirstName, LastName, Email, Username, Password, Role.USER).Value;

            // when
            user.Update("Update" + FirstName, "Update" + LastName, "Update" + Email);

            // then
            Assert.Equal("Update" + FirstName, user.FirstName);
            Assert.Equal("Update" + LastName, user.LastName);
            Assert.Equal("Update" + Email, user.Email);
            Assert.Equal(Username, user.Username);
            Assert.Equal(Password, user.Password);
            Assert.Equal(Role.USER, user.Role);
        }
    }
}

