using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Services.Error;

namespace WebApp.Controllers
{
    public class HomeController : AppController
    {
        public HomeController(IErrorService errorService) : base(errorService)
        {
        }

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

        [Authorize(Roles = "Admin")]
        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        [Authorize(Roles = "admin")]
        public IActionResult Privacy()
        {
            return View();
        }
    }
}
