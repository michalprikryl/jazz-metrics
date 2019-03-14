using Library.Models;
using Library.Models.Error;
using System.Threading.Tasks;

namespace WebAPI.Services.Helper
{
    public interface IHelperService
    {
        CurrentUser GetCurrentUser(int userId);
        Task<BaseResponseModel> SaveErrorToDB(ErrorModel value, int exceptionRound = 1);
    }
}
