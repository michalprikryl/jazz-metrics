using Library.Models;
using Library.Models.Projects;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebAPI.Services.Helper;
using WebAPI.Services.Projects;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : MainController
    {
        private readonly IProjectService _projectService;

        public ProjectController(IHelperService helperService, IProjectService projectService) : base(helperService)
        {
            _projectService = projectService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BaseResponseModelGet<ProjectModel>>> Get(int id, bool lazy = true)
        {
            return await _projectService.Get(id, lazy);
        }

        [HttpGet]
        public async Task<ActionResult<BaseResponseModelGetAll<ProjectModel>>> Get(bool lazy = true)
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
            return await _projectService.Drop(id);
        }
    }
}