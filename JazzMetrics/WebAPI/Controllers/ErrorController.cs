using Library.Models;
using Library.Models.Error;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebAPI.Services.Helper;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/error")]
    public class ErrorController : MainController
    {
        public ErrorController(IHelperService helperService) : base(helperService) { }

        [HttpPost, AllowAnonymous]
        public async Task<ActionResult<BaseResponseModel>> Post([FromBody]ErrorModel value)
        {
            return await HelperService.SaveErrorToDB(value);
        }
    }
}