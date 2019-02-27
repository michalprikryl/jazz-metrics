using Microsoft.AspNetCore.Mvc;
using WebAPI.Services.Error;

namespace WebAPI.Controllers
{
    public class MainController : ControllerBase
    {
        protected readonly IErrorService ErrorService;

        public MainController(IErrorService errorService)
        {
            ErrorService = errorService;
        }
    }
}