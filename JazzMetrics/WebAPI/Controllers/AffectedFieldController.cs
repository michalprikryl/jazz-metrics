using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<ActionResult<AffectedFieldModel>> Get(int id)
        {
            return await _affectedFieldService.Get(id);
        }

        [HttpGet]
        public async Task<ActionResult<BaseResponseModelGet<AffectedFieldModel>>> Get()
        {
            return await _affectedFieldService.GetAll();
        }

        [HttpPost]
        public async Task<ActionResult<BaseResponseModel>> Post(AffectedFieldModel model)
        {
            return await _affectedFieldService.Create(model);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BaseResponseModel>> Put(int id, AffectedFieldModel model)
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