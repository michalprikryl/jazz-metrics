using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebAPI.Models;
using WebAPI.Models.AffectedFields;
using WebAPI.Services.AffectedFields;
using WebAPI.Services.Error;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AffectedFieldController : MainController
    {
        private readonly IAffectedFieldService _affectedFieldService;

        public AffectedFieldController(IErrorService errorService, IAffectedFieldService affectedFieldService) : base(errorService) => _affectedFieldService = affectedFieldService;

        [HttpGet("{id}")]
        public async Task<ActionResult<AffectedFieldModel>> Get(int id, bool lazy = true)
        {
            return await _affectedFieldService.Get(id, lazy);
        }

        [HttpGet]
        public async Task<ActionResult<BaseResponseModelGet<AffectedFieldModel>>> Get(bool lazy = true)
        {
            return await _affectedFieldService.GetAll(lazy);
        }

        [HttpPost]
        public async Task<ActionResult<BaseResponseModelPost>> Post([FromBody]AffectedFieldModel model)
        {
            return await _affectedFieldService.Create(model);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BaseResponseModel>> Put(int id, [FromBody]AffectedFieldModel model)
        {
            return await _affectedFieldService.Edit(model);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<BaseResponseModel>> Delete(int id)
        {
            return await _affectedFieldService.Drop(id);
        }
    }
}