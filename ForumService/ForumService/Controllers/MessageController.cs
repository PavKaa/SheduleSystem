using Microsoft.AspNetCore.Mvc;

namespace ForumService.Controllers
{
	public class MessageController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
