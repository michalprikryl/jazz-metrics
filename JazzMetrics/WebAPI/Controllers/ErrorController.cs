using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;
using WebAPI.Models.Error;
using WebAPI.Services.Error;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/error")]
    public class ErrorController : MainController
    {
        public ErrorController(IErrorService errorService) : base(errorService) { }

        public async Task<ActionResult<BaseResponseModel>> Post([FromBody]ErrorModel value)
        {
            return await ErrorService.SaveErrorToDB(value);
        }
    }
}