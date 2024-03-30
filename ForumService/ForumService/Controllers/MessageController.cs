using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.DTO;
using Service.Interface;
using System.Net;

namespace ForumService.Controllers
{
	[Route("api/forum/")]
	[ApiController]
	public class MessageController : Controller
	{
		private readonly IMessageService _service;

		public MessageController(IMessageService service)
		{
			_service = service;
		}

		[HttpPost]
		[Route("messages")]
		public async Task<IActionResult> GetMessagesByTopic([FromQuery] long topicId)
		{
			if (ModelState.IsValid)
			{
				var response = await _service.GetByTopicAsync(topicId);

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
