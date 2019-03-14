using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Services.Helper;

namespace WebAPI.Controllers
{
    [Authorize]
    public class MainController : ControllerBase
    {
        public const string RoleUser = "user";
        public const string RoleAdmin = "admin";
        public const string RoleSuperAdmin = "super-admin";

        public IHelperService HelperService { get; }

        public MainController(IHelperService helperService)
        {
            HelperService = helperService;
        }
    }
}