using Library.Models;
using Library.Models.User;
using Library.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebAPI.Services.Helper;
using WebAPI.Services.Users;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : MainController
    {
        private readonly IUserService _userService;

        public LoginController(IHelperService helperService, IUserService userService) : base(helperService)
        {
            _userService = userService;
        }

        [HttpPost, AllowAnonymous]
        public async Task<ActionResult<BaseResponseModelGet<UserIdentityModel>>> Login([FromBody]LoginRequestModel value)
        {
            return await _userService.CheckUser(value);
        }
    }
}