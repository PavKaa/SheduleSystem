using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.DTO;
using Service.Interface;
using System.Net;

namespace ForumService.Controllers
{
	[Route("api/forum/")]
	[ApiController]
	public class TopicController : Controller
	{
		private readonly ITopicService _service;

		public TopicController(ITopicService service)
		{
			_service = service;
		}

		//[Authorize]
		[HttpPost]
		[Route("topics")]
		public async Task<IActionResult> AddTopic([FromBody] TopicDTO model)
		{
			if (ModelState.IsValid)
			{
				var response = await _service.CreateAsync(model);

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

		[HttpGet]
		[Route("topics")]
		public async Task<IActionResult> GetAllTopics()
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

		[HttpGet]
		[Route("topic")]
		public async Task<IActionResult> GetTopic([FromQuery] long id)
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
		[Route("topics")]
		public async Task<IActionResult> DeleteTopic([FromQuery] long id)
		{
			if (ModelState.IsValid)
			{
				var response = await _service.DeleteAsync(id); //TODO: make the check for correct user's id(is he the creator of topic)

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
		[Route("topics")]
		public async Task<IActionResult> UpdateTopic([FromQuery] TopicDTO model)
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
