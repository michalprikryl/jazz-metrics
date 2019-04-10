using Library.Models;
using Library.Models.Metric;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebAPI.Services.Helper;
using WebAPI.Services.Metrics;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = RoleSuperAdmin + "," + RoleAdmin)]
    public class MetricController : MainController
    {
        private readonly IMetricService _metricService;

        public MetricController(IHelperService helperService, IMetricService metricService) : base(helperService) => _metricService = metricService;

        [HttpGet("{id}")]
        public async Task<ActionResult<BaseResponseModelGet<MetricModel>>> Get(int id, bool lazy = true)
        {
            return await _metricService.Get(id, lazy);
        }

        [HttpGet]
        public async Task<ActionResult<BaseResponseModelGetAll<MetricModel>>> Get(bool lazy = true)
        {
            return await _metricService.GetAll(lazy);
        }

        [HttpPost]
        public async Task<ActionResult<BaseResponseModelPost>> Post([FromBody]MetricModel model)
        {
            return await _metricService.Create(model);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BaseResponseModel>> Put(int id, [FromBody]MetricModel model)
        {
            return await _metricService.Edit(model);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<BaseResponseModel>> Delete(int id)
        {
            return await _metricService.Drop(id);
        }
    }
}