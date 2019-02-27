using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebAPI.Services.Error;
using WebAPI.Services.Users;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TokenController : MainController
    {
        private readonly IUserService _userService;

        public TokenController(IErrorService errorService, IUserService userService) : base(errorService)
        {
            _userService = userService;
        }

        public async Task<ActionResult<object>> Post()
        {
            return new { Token = await _userService.BuildToken(UserIdentity.GetUsername()) };
        }
    }
}