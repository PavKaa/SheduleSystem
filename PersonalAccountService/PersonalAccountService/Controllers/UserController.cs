using BusinessLogic.DTO;
using BusinessLogic.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace PersonalAccountService.Controllers
{
	[ApiController]
	[Route("/api/pesonalaccount/")]
	public class UserController : Controller
	{
		private readonly IUserService userService;
		private readonly ILogger<UserController> logger;

		public UserController(IUserService userService, ILogger<UserController> logger)
        {
            this.userService = userService;
			this.logger = logger;

			logger.LogDebug(2, "NLog injected into UserController");
		}

		[AllowAnonymous]
		[Route("users")]
		[HttpPost]
		public async Task<ActionResult> AddNewUser([FromBody] CreateUserDTO model)
		{
			logger.LogInformation("Adding new user request", model);

			if (ModelState.IsValid)
			{
				var response = await userService.Create(model);

				if(response.StatusCode == HttpStatusCode.OK)
				{
					logger.LogInformation("Adding new user request: successful added new user", response.Data);
					return Ok();
				}

				logger.LogWarning("Adding new user request: internal server error while adding new user", response.Message);
				return StatusCode((int)response.StatusCode, response.Message);
			}
			else
			{
				logger.LogInformation("Adding new user request: incorrect request", model);
				return BadRequest();
			}
		}

		[Authorize]
		[Route("users")]
		[HttpGet]
		public async Task<ActionResult> GetUser([FromQuery] string id)
		{
			logger.LogInformation("Getting user by id request", id);

			if (ModelState.IsValid)
			{
				var response = await userService.Find(id);

				if (response.StatusCode == HttpStatusCode.OK)
				{
					logger.LogInformation("Getting user by id request: successful got user by id", response.Data);
					return Json(response.Data);
				}

				logger.LogWarning("Getting user by id request: internal server error while getting user", response.Message);
				return StatusCode((int)response.StatusCode, response.Message);
			}
			else
			{
				logger.LogInformation("Getting user by id request: incorrect request", id);
				return BadRequest();
			}
		}

		[Authorize]
		[Route("users")]
		[HttpDelete]
		public async Task<ActionResult> DeleteUser([FromQuery] string id)
		{
			logger.LogInformation("Deleting user by id request", id);

			if (ModelState.IsValid)
			{
				var response = await userService.Remove(id);

				if (response.StatusCode == HttpStatusCode.OK)
				{
					logger.LogInformation("Deleting user by id request: successful deleted user by id", response.Data);
					return Ok();
				}

				logger.LogWarning("Deleting user by id request: internal server error while deleting user", response.Message);
				return StatusCode((int)response.StatusCode, response.Message);
			}
			else
			{
				logger.LogInformation("Deleting user by id request: incorrect request", id);
				return BadRequest();
			}
		}

		[Authorize]
		[Route("users")]
		[HttpPut]
		public async Task<ActionResult> UpdateUser([FromBody] UserDTO model)
		{
			logger.LogInformation("Updating user request", model);

			if (ModelState.IsValid)
			{
				var response = await userService.Update(model);

				if (response.StatusCode == HttpStatusCode.OK)
				{
					logger.LogInformation("Updating user request: successful updated user", response.Data);
					return Json(response.Data);
				}

				logger.LogWarning("Updating user request: internal server error while updating user", response.Message);
				return StatusCode((int)response.StatusCode, response.Message);
			}
			else
			{
				logger.LogInformation("Updating user request: incorrect request", model);
				return BadRequest();
			}
		}
	}
}
