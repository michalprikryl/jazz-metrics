using Library.Models;
using Library.Models.Error;
using System.Threading.Tasks;

namespace WebApp.Services.Error
{
    public interface IErrorService
    {
        Task<BaseResponseModel> CreateError(ErrorModel model);
    }
}
