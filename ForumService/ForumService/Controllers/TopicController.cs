using Microsoft.AspNetCore.Mvc;

namespace ForumService.Controllers
{
	public class TopicController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
