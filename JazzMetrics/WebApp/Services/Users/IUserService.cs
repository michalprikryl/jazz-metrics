using Library.Models;
using Library.Models.User;
using System.Threading.Tasks;
using WebApp.Models.User;

namespace WebApp.Services.Users
{
    public interface IUserService
    {
        Task<BaseResponseModelGet<UserIdentityModel>> AuthenticateUser(LoginViewModel model);
        Task<BaseResponseModelPost> FindUserIdByUsername(string username, string jwt);
    }
}
