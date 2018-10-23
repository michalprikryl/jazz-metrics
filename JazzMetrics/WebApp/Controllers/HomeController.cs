using System.Web.Mvc;
using WebApp.Identity;

namespace WebApp.Controllers
{
    [CustomAuthorize(Roles = "admin")]
    public class HomeController : AppController
    {
        public ActionResult Index()
        {
            ViewBag.Information = $"Jste přihlášen jako {User.FirstName} {User.LastName}, email - {User.Email}, role - {string.Join(", ", User.Roles)}.";

            return View();
        }
    }
}