using BusinessLogic.DTO;
using BusinessLogic.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace PersonalAccountService.Controllers
{
	[ApiController]
	[Route("/api/pesonalaccount/")]
	public class UserDataController : Controller
	{
		private readonly IUserDataService userDataService;
		private readonly ILogger<UserDataController> logger;

		public UserDataController(IUserDataService userDataService, ILogger<UserDataController> logger)
		{
			this.userDataService = userDataService;
			this.logger = logger;

			logger.LogDebug(3, "NLog injected into UserDataController");
		}

		[Authorize]
		[Route("userdata")]
		[HttpPost]
		public async Task<ActionResult> AddNewUserData([FromBody] UserDataDTO model)
		{
			logger.LogInformation("Adding new user data request", model);

			if (ModelState.IsValid)
			{
				var response = await userDataService.Create(model);

				if (response.StatusCode == HttpStatusCode.OK)
				{
					logger.LogInformation("Adding new user data request: successful added new user data", response.Data);
					return Json(response.Data);
				}

				logger.LogWarning("Adding new user data request: internal server error while adding new user data", response.Message);
				return StatusCode((int)response.StatusCode, response.Message);
			}
			else
			{
				logger.LogInformation("Adding new user data request: incorrect request", model);
				return BadRequest();
			}
		}

		[Authorize]
		[Route("userdata")]
		[HttpGet]
		public async Task<ActionResult> GetUserData([FromQuery] string id)
		{
			logger.LogInformation("Getting user data by id request", id);

			if (ModelState.IsValid)
			{
				var response = await userDataService.Find(id);

				if (response.StatusCode == HttpStatusCode.OK)
				{
					logger.LogInformation("Getting user data by id request: successful got user data by id", response.Data);
					return Json(response.Data);
				}

				logger.LogWarning("Getting user data by id request: internal server error while getting user data", response.Message);
				return StatusCode((int)response.StatusCode, response.Message);
			}
			else
			{
				logger.LogInformation("Getting user data by id request: incorrect request", id);
				return BadRequest();
			}
		}

		[Authorize]
		[Route("userdata")]
		[HttpDelete]
		public async Task<ActionResult> DeleteUserData([FromQuery] string id)
		{
			logger.LogInformation("Deleting user data by id request", id);

			if (ModelState.IsValid)
			{
				var response = await userDataService.Remove(id);

				if (response.StatusCode == HttpStatusCode.OK)
				{
					logger.LogInformation("Deleting user data by id request: successful deleted user data by id", response.Data);
					return Ok();
				}

				logger.LogWarning("Deleting user data by id request: internal server error while deleting user data", response.Message);
				return StatusCode((int)response.StatusCode, response.Message);
			}
			else
			{
				logger.LogInformation("Deleting user data by id request: incorrect request", id);
				return BadRequest();
			}
		}

		[Authorize]
		[Route("userdata")]
		[HttpPut]
		public async Task<ActionResult> UpdateUserData([FromBody] UserDataDTO model)
		{
			logger.LogInformation("Updating user data request", model);

			if (ModelState.IsValid)
			{
				var response = await userDataService.Update(model);

				if (response.StatusCode == HttpStatusCode.OK)
				{
					logger.LogInformation("Updating user data request: successful updated user data", response.Data);
					return Json(response.Data);
				}

				logger.LogWarning("Updating user data request: internal server error while updating user data", response.Message);
				return StatusCode((int)response.StatusCode, response.Message);
			}
			else
			{
				logger.LogInformation("Updating user data request: incorrect request", model);
				return BadRequest();
			}
		}
	}
}
