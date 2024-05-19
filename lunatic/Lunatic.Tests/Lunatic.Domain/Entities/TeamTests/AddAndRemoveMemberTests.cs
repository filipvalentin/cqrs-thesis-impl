
using Lunatic.Domain.Entities;


namespace Tests.Lunatic.Domain.Entities.TeamTests {
	public class AddAndRemoveMemberTests {
		public const string TeamName = "Lunatic";
		public const string TeamDescription = "Lunatic Description";

		public const string UserFirstName = "John";
		public const string UserLastName = "Smith";
		public const string UserEmail = "smith@gmail.com";
		public const string UserUsername = "smith";
		public const string UserPassword = "String123#";
		public const Role UserRole = Role.USER;

		[Fact]
		public void GivenTeamWithUser_WhenAddAndRemoveMember_ThenMemberAddedAndRemovedSuccessfully() {
			// given
			var team = Team.Create(Guid.NewGuid(), TeamName, TeamDescription).Value;
			var user = User.Create(UserFirstName, UserLastName, UserEmail, UserUsername, UserPassword, UserRole).Value;
			var userId = Guid.NewGuid();

			// when
			//team.AddMember(user);
			team.AddMember(userId);
			//team.RemoveMember(user);
			team.RemoveMember(userId);

			// then
			Assert.DoesNotContain(user.Id, team.MemberIds);
			Assert.DoesNotContain(userId, team.MemberIds);
		}
	}
}

