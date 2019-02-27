using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Services.Error;

namespace WebAPI.Controllers
{
    [Authorize]
    public class MainController : ControllerBase
    {
        protected readonly IErrorService ErrorService;

        public MainController(IErrorService errorService)
        {
            ErrorService = errorService;
        }
    }
}