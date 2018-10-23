using WebAPI.Classes.Error;
using WebAPI.Models;
using WebAPI.Models.Error;
using System.Threading.Tasks;
using System.Web.Http;

namespace WebAPI.Controllers
{
    public class ErrorController : ApiController
    {
        public async Task<BaseResponseModel> Post([FromBody]ErrorModel value)
        {
            return await ErrorManager.SaveErrorToDB(value);
        }
    }
}
