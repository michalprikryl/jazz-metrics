using Library;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebAPI.Services.Helper;
using WebAPI.Services.Users;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TokenController : MainController
    {
        private readonly IUserService _userService;

        public TokenController(IHelperService helperService, IUserService userService) : base(helperService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<ActionResult<object>> Post()
        {
            return new { Token = await _userService.BuildToken(User.GetId(), User.GetUserRole()) };
        }
    }
}