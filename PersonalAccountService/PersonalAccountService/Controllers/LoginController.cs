using BusinessLogic.DTO;
using BusinessLogic.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace PersonalAccountService.Controllers
{
	[ApiController]
	[Route("/api/pesonalaccount/")]
	public class LoginController : Controller
	{
		private readonly ILoginService loginService;
		private readonly ILogger<LoginController> logger;

		public LoginController(ILoginService loginService, ILogger<LoginController> logger)
        {
            this.loginService = loginService;
			this.logger = logger;

			logger.LogDebug(1, "NLog injected into LoginController");
		}

		[AllowAnonymous]
		[HttpPost]
		[Route(("login"))]
		public async Task<IActionResult> Login([FromBody] UserLoginDTO userLoginModel)
		{
			logger.LogInformation("Login Request", userLoginModel);

			if (ModelState.IsValid)
			{
				var user = await loginService.Authenticate(userLoginModel);

				if(user == null) 
				{
					logger.LogInformation("Login request: incorrect login information", userLoginModel);
					return BadRequest("Incorrect login or password");
				}

				var response = await loginService.GenerateTokens(user);

				if(response.StatusCode == HttpStatusCode.InternalServerError)
				{
					logger.LogWarning("Login request: internal server error in login method", response.Message);
					return StatusCode((int)HttpStatusCode.InternalServerError);
				}

				setTokenCookie(response.RefreshToken);

				logger.LogInformation("Login request: successful login", userLoginModel);

				return Ok(response);
			}
			else 
			{
				logger.LogInformation("Login request: uncorrect data", userLoginModel);
				return BadRequest();
			}
		}

		[AllowAnonymous]
		[HttpPost]
		[Route(("refresh-token"))]
		public async Task<IActionResult> RefreshToken()
		{
			logger.LogInformation("Refresh token request");

			var refreshToken = Request.Cookies["refreshToken"];

			if(refreshToken == null)
			{
				logger.LogInformation("Refresh token request: incorrect refresh token");
				return BadRequest();
			}

			var response = await loginService.RefreshTokens(refreshToken);

			if(response.StatusCode != HttpStatusCode.OK) 
			{
				logger.LogWarning("Refresh token request: error with handling the refresh token", response.Message);
				return StatusCode((int)response.StatusCode);
			}

			setTokenCookie(response.RefreshToken);

			logger.LogInformation("Refresh token request: successful refresh tokens");

			return Ok(response);
		}

		private void setTokenCookie(string refreshToken)
		{
			var cookieOptions = new CookieOptions
			{
				HttpOnly = true,
				Expires = DateTime.UtcNow.AddDays(7)
			};

			Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
		}
	}
}
