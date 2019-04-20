using Library.Models;
using Library.Models.Settings;
using Library.Networking;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Services.Helper;
using WebAPI.Services.Settings;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = RoleSuperAdmin)]
    public class SettingController : MainController
    {
        private readonly ISettingService _settingService;

        public SettingController(IHelperService helperService, ISettingService settingService) : base(helperService)
        {
            _settingService = settingService;
        }

        [HttpGet]
        public async Task<ActionResult<BaseResponseModelGetAll<SettingModel>>> Get(bool lazy = true)
        {
            return await _settingService.GetAll(lazy);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<BaseResponseModel>> Patch(int id, [FromBody]List<PatchModel> values)
        {
            return await _settingService.PartialEdit(id, values);
        }
    }
}