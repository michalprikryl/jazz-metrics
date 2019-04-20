using Library.Models;
using Library.Models.AppError;
using System.Threading.Tasks;

namespace WebAPI.Services.Helper
{
    public interface IHelperService
    {
        CurrentUser GetCurrentUser(int userId);
        Task<BaseResponseModel> SaveErrorToDB(AppErrorModel value, int exceptionRound = 1);
    }
}
