using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Services.Error;

namespace WebApp.Controllers
{
    public class HomeController : AppController
    {
        public HomeController(IErrorService errorService) : base(errorService) { }

        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult About()
        {
            int[] u = new int[] { 1 };
            u[51] = 10;

            ViewData["Message"] = "Your application description page.";

            return View();
        }
    }
}
