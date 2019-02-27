using System.Threading.Tasks;
using WebAPI.Models;
using WebAPI.Models.Error;

namespace WebAPI.Services.Error
{
    public interface IErrorService
    {
        Task<BaseResponseModel> SaveErrorToDB(ErrorModel value, int exceptionRound = 1);
    }
}
