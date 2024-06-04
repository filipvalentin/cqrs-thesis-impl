
using Lunatic.Identity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace Lunatic.Identity {
	public class LunaticUserContext : IdentityDbContext<ApplicationUser> {
		public LunaticUserContext(DbContextOptions<LunaticUserContext> options) : base(options) { }
	}
}
