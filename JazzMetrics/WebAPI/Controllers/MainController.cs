using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Services.Error;

namespace WebAPI.Controllers
{
    [Authorize]
    public class MainController : ControllerBase
    {
        public IErrorService ErrorService { get; }

        public MainController(IErrorService errorService)
        {
            ErrorService = errorService;
        }
    }
}