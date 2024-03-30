using Domain.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;
using System.Net;

namespace ForumService.Controllers
{
	[Route("api/forum/")]
	[ApiController]
	public class FileController : Controller
	{
		private readonly IFileService _fileSerive;
		IWebHostEnvironment _appEnvironment;

		public FileController(IFileService fileSerive, IWebHostEnvironment appEnvironment)
		{
			_fileSerive = fileSerive;
			_appEnvironment = appEnvironment;
		}

		[Authorize]
		[HttpPost]
		[Route("files")]
		public async Task<IActionResult> AddFile(IFormFile uploadedFile)
		{
			if (ModelState.IsValid)
			{
				var response = await _fileSerive.CreateAsync(uploadedFile, _appEnvironment.WebRootPath);

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
