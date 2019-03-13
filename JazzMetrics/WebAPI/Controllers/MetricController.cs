using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebAPI.Models;
using WebAPI.Models.Metric;
using WebAPI.Services.Error;
using WebAPI.Services.Metrics;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MetricController : MainController
    {
        private readonly IMetricService _metricService;

        public MetricController(IErrorService errorService, IMetricService metricService) : base(errorService) => _metricService = metricService;

        [HttpGet("{id}")]
        public async Task<ActionResult<MetricModel>> Get(int id, bool lazy = true)
        {
            return await _metricService.Get(id, lazy);
        }

        [HttpGet]
        public async Task<ActionResult<BaseResponseModelGet<MetricModel>>> Get(bool lazy = true)
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
            return await _metricService.DropAsync(id);
        }
    }
}