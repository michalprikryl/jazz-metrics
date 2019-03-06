using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;
using WebAPI.Models.MetricType;
using WebAPI.Services.Error;
using WebAPI.Services.MetricTypes;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MetricTypeController : MainController
    {
        private readonly IMetricTypeService _metricTypeService;

        public MetricTypeController(IErrorService errorService, IMetricTypeService metricTypeService) : base(errorService) => _metricTypeService = metricTypeService;

        [HttpGet("{id}")]
        public async Task<ActionResult<MetricTypeModel>> Get(int id)
        {
            return await _metricTypeService.Get(id);
        }

        [HttpGet]
        public async Task<ActionResult<BaseResponseModelGet<MetricTypeModel>>> Get()
        {
            return await _metricTypeService.GetAll();
        }

        [HttpPost]
        public async Task<ActionResult<BaseResponseModel>> Post(MetricTypeModel model)
        {
            return await _metricTypeService.Create(model);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BaseResponseModel>> Put(int id, MetricTypeModel model)
        {
            return await _metricTypeService.Edit(model);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<BaseResponseModel>> Delete(int id)
        {
            return await _metricTypeService.Drop(id);
        }
    }
}