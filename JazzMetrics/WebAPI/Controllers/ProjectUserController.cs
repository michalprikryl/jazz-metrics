using Library.Models;
using Library.Models.ProjectUsers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebAPI.Services.Helper;
using WebAPI.Services.ProjectUsers;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = RoleSuperAdmin + "," + RoleAdmin)]
    public class ProjectUserController : MainController
    {
        private readonly IProjectUserService _projectUserService;

        public ProjectUserController(IHelperService helperService, IProjectUserService projectUserService) : base(helperService)
        {
            _projectUserService = projectUserService;
        }

        [HttpGet]
        public async Task<ActionResult<BaseResponseModelGet<ProjectUserModel>>> Get(int projectId, int userId)
        {
            return await _projectUserService.GetByProjectAndUser(projectId, userId);
        }

        [HttpPost]
        public async Task<ActionResult<BaseResponseModelPost>> Post([FromBody]ProjectUserModel model)
        {
            return await _projectUserService.Create(model);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<BaseResponseModel>> Delete(int id)
        {
            return await _projectUserService.Drop(id);
        }
    }
}