using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApp.Models;
using WebApp.Services.Error;
using WebApp.Services.User;

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
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModelOld { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
