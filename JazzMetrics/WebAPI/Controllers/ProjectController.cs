using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebAPI.Models;
using WebAPI.Models.Projects;
using WebAPI.Services.Error;
using WebAPI.Services.Projects;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : MainController
    {
        private readonly IProjectService _projectService;

        public ProjectController(IErrorService errorService, IProjectService projectService, IHttpContextAccessor contextAccessor) : base(errorService)
        {
            _projectService = projectService;
            _projectService.CurrentUserId = contextAccessor.HttpContext.User.GetId();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectModel>> Get(int id, bool lazy = true)
        {
            return await _projectService.Get(id, lazy);
        }

        [HttpGet]
        public async Task<ActionResult<BaseResponseModelGet<ProjectModel>>> Get(bool lazy = true)
        {
            return await _projectService.GetAll(lazy);
        }

        [HttpPost]
        public async Task<ActionResult<BaseResponseModelPost>> Post([FromBody]ProjectModel model)
        {
            return await _projectService.Create(model);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BaseResponseModel>> Put(int id, [FromBody]ProjectModel model)
        {
            return await _projectService.Edit(model);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<BaseResponseModel>> Delete(int id)
        {
            return await _projectService.DropAsync(id);
        }
    }
}