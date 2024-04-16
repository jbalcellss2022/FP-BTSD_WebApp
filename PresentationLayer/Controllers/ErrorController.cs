using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers
{
	public class ErrorController : Controller
	{
		[Route("Error")]
		public IActionResult Error()
		{
			var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

			if (exceptionFeature != null)
			{
				// exceptionFeature.Error
			}
			return View();
		}

		public IActionResult ThrowError()
		{
			throw new Exception("Launch Exception Tester.");
		}

	}
}
