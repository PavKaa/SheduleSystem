using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.DTO;
using Service.Interface;
using System.Net;

namespace ForumService.Controllers
{
	[Route("api/forum/")]
	[ApiController]
	public class UserController : Controller
	{
		private readonly IUserService _service;

		public UserController(IUserService service)
		{
			_service = service;
		}

		//[Authorize]
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

		[Authorize]
		[HttpGet]
		[Route("users")]
		public async Task<IActionResult> GetAllUsers()
		{
			if (ModelState.IsValid)
			{
				var response = await _service.GetAllAsync();

				if (response.StatusCode != HttpStatusCode.OK)
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

		[Authorize]
		[HttpGet]
		[Route("user")]
		public async Task<IActionResult> GetUser([FromQuery] long id)
		{
			if (ModelState.IsValid)
			{
				var response = await _service.GetAsync(id);

				if (response.StatusCode != HttpStatusCode.OK)
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

		[Authorize]
		[HttpDelete]
		[Route("users")]
		public async Task<IActionResult> DeleteUser([FromQuery] long id)
		{
			if (ModelState.IsValid)
			{
				var response = await _service.DeleteAsync(id);

				if (response.StatusCode != HttpStatusCode.OK)
				{
					return StatusCode((int)response.StatusCode, response.Message);
				}

				return Ok();
			}
			else
			{
				return BadRequest();
			}
		}

		[Authorize]
		[HttpPut]
		[Route("users")]
		public async Task<IActionResult> UpdateUser([FromQuery] UserDTO model)
		{
			if (ModelState.IsValid)
			{
				var response = await _service.UpdateAsync(model);

				if (response.StatusCode != HttpStatusCode.OK)
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
