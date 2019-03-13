using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Services.Error;

namespace WebAPI.Controllers
{
    [Authorize]
    public class MainController : ControllerBase
    {
        public const string RoleUser = "user";
        public const string RoleAdmin = "admin";
        public const string RoleSuperAdmin = "super-admin";

        public IErrorService ErrorService { get; }

        public MainController(IErrorService errorService)
        {
            ErrorService = errorService;
        }
    }
}