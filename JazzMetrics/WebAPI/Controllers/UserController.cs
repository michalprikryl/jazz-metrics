using Library.Networking;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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

        [HttpGet]
        public async Task<ActionResult<BaseResponseModelPost>> GetByUsername(string username)
        {
            return await _userService.GetByUsername(username);
        }

        [HttpPost, AllowAnonymous]
        public async Task<ActionResult<BaseResponseModel>> Post([FromBody]UserModel value)
        {
            return await _userService.Create(value);
        }

        [HttpPost("Login"), AllowAnonymous]
        public async Task<ActionResult<LoginResponseModel>> Login([FromBody]LoginRequestModel value)
        {
            return await _userService.CheckUser(value);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<BaseResponseModel>> Patch(int id, [FromBody]List<PatchModel> values)
        {
            return await _userService.PartialEdit(id, values);
        }
    }
}