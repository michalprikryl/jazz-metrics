using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
using WebApp.Models.Error;
using WebApp.Services.Error;

namespace WebApp.Controllers
{
    [AllowAnonymous]
    public class ErrorController : AppController
    {
        public ErrorController(IErrorService errorService) : base(errorService) { }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Index()
        {
            var feature = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var error = feature?.Error;
            if (error != null)
            {
               var task = ErrorService.CreateError(new ErrorModel(error, MyUser?.UserId.ToString(), "global error handler"));
            }

            return View(new ErrorViewModel { RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        [Route("/error/404")]
        public new IActionResult NotFound()
        {
            return View();
        }
    }
}