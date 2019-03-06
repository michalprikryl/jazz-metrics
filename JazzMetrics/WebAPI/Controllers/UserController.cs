using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebAPI.Models;
using WebAPI.Models.Users;
using WebAPI.Services.Error;
using WebAPI.Services.Users;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : MainController
    {
        private readonly IUserService _userService;

        public UserController(IErrorService errorService, IUserService userService) : base(errorService) => _userService = userService;

        [HttpPost, AllowAnonymous]
        public async Task<ActionResult<BaseResponseModel>> Post([FromBody]RegistrationRequestModel value)
        {
            return await _userService.Registration(value);
        }

        [HttpPost("Login"), AllowAnonymous]
        public async Task<ActionResult<LoginResponseModel>> Login([FromBody]LoginRequestModel value)
        {
            return await _userService.CheckUser(value);
        }
    }
}