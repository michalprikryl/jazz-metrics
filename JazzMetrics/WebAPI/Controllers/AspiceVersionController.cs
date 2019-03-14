using Library.Models;
using Library.Models.AspiceVersions;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebAPI.Services.AspiceVersions;
using WebAPI.Services.Helper;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AspiceVersionController : MainController
    {
        private readonly IAspiceVersionService _aspiceVersionService;

        public AspiceVersionController(IHelperService helperService, IAspiceVersionService aspiceVersionService) : base(helperService) => _aspiceVersionService = aspiceVersionService;

        [HttpGet("{id}")]
        public async Task<ActionResult<BaseResponseModelGet<AspiceVersionModel>>> Get(int id, bool lazy = true)
        {
            return await _aspiceVersionService.Get(id, lazy);
        }

        [HttpGet]
        public async Task<ActionResult<BaseResponseModelGetAll<AspiceVersionModel>>> Get(bool lazy = true)
        {
            return await _aspiceVersionService.GetAll(lazy);
        }

        [HttpPost]
        public async Task<ActionResult<BaseResponseModelPost>> Post([FromBody]AspiceVersionModel model)
        {
            return await _aspiceVersionService.Create(model);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BaseResponseModel>> Put(int id, [FromBody]AspiceVersionModel model)
        {
            return await _aspiceVersionService.Edit(model);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<BaseResponseModel>> Delete(int id)
        {
            return await _aspiceVersionService.Drop(id);
        }
    }
}