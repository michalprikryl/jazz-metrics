using System.Threading.Tasks;
using WebApp.Models;
using WebApp.Models.Error;

namespace WebApp.Services.Error
{
    public interface IErrorService
    {
        Task<BaseApiResult> CreateError(ErrorJsModel model, string userID);
        Task<BaseApiResult> CreateError(ErrorModel model);
    }
}
