using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebAPI.Services.Error;

namespace WebAPI.Controllers
{
    [Authorize]
    public class MainController : ControllerBase
    {
        protected readonly IErrorService ErrorService;

        /// <summary>
        /// vrati identitu aktualniho uzivatele, jsou vyplnene name (username) a role (workplace)
        /// </summary>
        public ClaimsIdentity UserIdentity
        {
            get
            {
                return User.Identity as ClaimsIdentity;
            }
        }

        public MainController(IErrorService errorService)
        {
            ErrorService = errorService;
        }
    }
}