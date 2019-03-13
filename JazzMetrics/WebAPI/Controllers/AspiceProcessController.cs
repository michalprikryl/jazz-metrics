using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebAPI.Models;
using WebAPI.Models.AspiceProcesses;
using WebAPI.Services.AspiceProcesses;
using WebAPI.Services.Error;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AspiceProcessController : MainController
    {
        private readonly IAspiceProcessService _aspiceProcessService;

        public AspiceProcessController(IErrorService errorService, IAspiceProcessService aspiceVersionService) : base(errorService) => _aspiceProcessService = aspiceVersionService;

        [HttpGet("{id}")]
        public async Task<ActionResult<AspiceProcessModel>> Get(int id, bool lazy = true)
        {
            return await _aspiceProcessService.Get(id, lazy);
        }

        [HttpGet]
        public async Task<ActionResult<BaseResponseModelGet<AspiceProcessModel>>> Get(bool lazy = true)
        {
            return await _aspiceProcessService.GetAll(lazy);
        }

        [HttpPost]
        public async Task<ActionResult<BaseResponseModelPost>> Post([FromBody]AspiceProcessModel model)
        {
            return await _aspiceProcessService.Create(model);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BaseResponseModel>> Put(int id, [FromBody]AspiceProcessModel model)
        {
            return await _aspiceProcessService.Edit(model);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<BaseResponseModel>> Delete(int id)
        {
            return await _aspiceProcessService.DropAsync(id);
        }
    }
}