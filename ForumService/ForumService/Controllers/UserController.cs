using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.DTO;
using Service.Interface;
using System.Net;

namespace ForumService.Controller
{
	[Route("api/forum/")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly IUserService _service;

		public UserController(IUserService service)
		{
			_service = service;
		}

		[Authorize]
		[HttpPost]
		[Route("users")]
		public async Task<IActionResult> AddUser([FromBody] UserDTO model)
		{
			if(ModelState.IsValid)
			{
				var response = await _service.CreateAsync(model);

				if(response.StatusCode != HttpStatusCode.OK)
				{
					return StatusCode((int)response.StatusCode, response.Message);
				}


				return Json(response.Data);
			}
			else
			{
				return BadRequest();
			}
		}
    }
}
