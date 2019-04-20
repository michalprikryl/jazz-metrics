using Library.Models;
using Library.Models.AppError;
using System.Threading.Tasks;

namespace WebApp.Services.Error
{
    public interface IErrorService
    {
        Task<BaseResponseModel> CreateError(AppErrorModel model);
    }
}
