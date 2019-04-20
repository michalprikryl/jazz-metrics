using Library.Models;
using Library.Models.AffectedFields;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebAPI.Services.AffectedFields;
using WebAPI.Services.Helper;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AffectedFieldController : MainController
    {
        private readonly IAffectedFieldService _affectedFieldService;

        public AffectedFieldController(IHelperService helperService, IAffectedFieldService affectedFieldService) : base(helperService) => _affectedFieldService = affectedFieldService;

        [HttpGet("{id}")]
        [Authorize(Roles = RoleSuperAdmin + "," + RoleAdmin)]
        public async Task<ActionResult<BaseResponseModelGet<AffectedFieldModel>>> Get(int id, bool lazy = true)
        {
            return await _affectedFieldService.Get(id, lazy);
        }

        [HttpGet]
        [Authorize(Roles = RoleSuperAdmin + "," + RoleAdmin)]
        public async Task<ActionResult<BaseResponseModelGetAll<AffectedFieldModel>>> Get(bool lazy = true)
        {
            return await _affectedFieldService.GetAll(lazy);
        }

        [HttpPost]
        [Authorize(Roles = RoleSuperAdmin)]
        public async Task<ActionResult<BaseResponseModelPost>> Post([FromBody]AffectedFieldModel model)
        {
            return await _affectedFieldService.Create(model);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = RoleSuperAdmin)]
        public async Task<ActionResult<BaseResponseModel>> Put(int id, [FromBody]AffectedFieldModel model)
        {
            return await _affectedFieldService.Edit(model);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = RoleSuperAdmin)]
        public async Task<ActionResult<BaseResponseModel>> Delete(int id)
        {
            return await _affectedFieldService.Drop(id);
        }
    }
}