using Library.Models;
using Library.Models.ProjectMetricLogs;
using Library.Models.ProjectMetrics;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebAPI.Services.Helper;
using WebAPI.Services.ProjectMetrics;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectMetricController : MainController
    {
        private readonly IProjectMetricService _projectMetricService;

        public ProjectMetricController(IHelperService helperService, IProjectMetricService projectMetricService) : base(helperService) => _projectMetricService = projectMetricService;

        [HttpGet("{id}")]
        public async Task<ActionResult<BaseResponseModelGet<ProjectMetricModel>>> Get(int id, bool lazy = true)
        {
            return await _projectMetricService.Get(id, lazy);
        }

        [HttpGet]
        public async Task<ActionResult<BaseResponseModelGetAll<ProjectMetricModel>>> GetByProjectId(int projectId, bool lazy = true)
        {
            return await _projectMetricService.GetAllByProjectId(projectId, lazy);
        }

        [HttpPost]
        public async Task<ActionResult<BaseResponseModelPost>> Post([FromBody]ProjectMetricModel model)
        {
            return await _projectMetricService.Create(model);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BaseResponseModel>> Put(int id, [FromBody]ProjectMetricModel model)
        {
            return await _projectMetricService.Edit(model);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<BaseResponseModel>> Delete(int id)
        {
            return await _projectMetricService.Drop(id);
        }

        [HttpGet("{id}/ProjectMetricLog")]
        public async Task<ActionResult<BaseResponseModelGetAll<ProjectMetricLogModel>>> Get(int id)
        {
            return await _projectMetricService.GetProjectMetricLogs(id);
        }
    }
}