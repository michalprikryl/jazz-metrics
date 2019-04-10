using Library.Models;
using Library.Models.Users;
using Library.Networking;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Services.Helper;
using WebAPI.Services.Users;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : MainController
    {
        private readonly IUserService _userService;

        public UserController(IHelperService helperService, IUserService userService) : base(helperService) => _userService = userService;

        [HttpGet("{id}")]
        public async Task<ActionResult<BaseResponseModelGet<UserModel>>> Get(int id, bool lazy = true)
        {
            return await _userService.Get(id, lazy);
        }

        [HttpGet]
        public async Task<ActionResult<BaseResponseModelPost>> GetByUsername(string username)
        {
            return await _userService.GetByUsername(username);
        }

        [HttpPost, AllowAnonymous]
        [Authorize(Roles = RoleSuperAdmin + "," + RoleAdmin)]
        public async Task<ActionResult<BaseResponseModel>> Post([FromBody]UserModel value)
        {
            return await _userService.Create(value);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BaseResponseModel>> Put(int id, [FromBody]UserModel value)
        {
            return await _userService.Edit(value);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<BaseResponseModel>> Patch(int id, [FromBody]List<PatchModel> values)
        {
            return await _userService.PartialEdit(id, values);
        }
    }
}