
using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace Lunatic.API.Controllers {
	[Route("api/v1/[controller]")]
	[ApiController]
	public class BaseApiController : ControllerBase {
		private ISender mediator = null!;
		protected ISender Mediator => mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
	}
}
