using WebApp.Classes;
using WebApp.Classes.Error;
using WebApp.Models.Error;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class ErrorController : AppController
    {
        public ActionResult Lockout()
        {
            return View();
        }

        public ActionResult NotFound()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> CreateError(ErrorModel model)
        {
            BaseAPIResult result = new BaseAPIResult { Success = true };

            try
            {
                ErrorSender sender = new ErrorSender();
                result = await sender.CreateError(model, User?.UserId.ToString() ?? "WA");
            }
            catch
            {
                result.Success = false;
            }

            return Json(result);
        }
    }
}