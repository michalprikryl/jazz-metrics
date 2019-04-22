using Library.Models;
using Library.Models.AppError;
using Library.Networking;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Services.AppErrors;
using WebAPI.Services.Helper;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/apperror")]
    public class AppErrorController : MainController
    {
        private readonly IAppErrorService _appErrorService;

        public AppErrorController(IHelperService helperService, IAppErrorService appErrorService) : base(helperService)
        {
            _appErrorService = appErrorService;
        }

        [HttpGet]
        [Authorize(Roles = RoleSuperAdmin)]
        public async Task<ActionResult<BaseResponseModelGetAll<AppErrorModel>>> Get(bool lazy = true)
        {
            return await _appErrorService.GetAll(lazy);
        }

        [HttpPost, AllowAnonymous]
        public async Task<ActionResult<BaseResponseModel>> Post([FromBody]AppErrorModel value)
        {
            return await HelperService.SaveErrorToDB(value);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = RoleSuperAdmin)]
        public async Task<ActionResult<BaseResponseModel>> Delete(int id)
        {
            return await _appErrorService.Drop(id);
        }

        [HttpPatch("{id}")]
        [Authorize(Roles = RoleSuperAdmin)]
        public async Task<ActionResult<BaseResponseModel>> Patch(int id, [FromBody]List<PatchModel> values)
        {
            return await _appErrorService.PartialEdit(id, values);
        }
    }
}