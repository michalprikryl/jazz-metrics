using System.Threading.Tasks;
using WebApp.Models;
using WebApp.Models.User;

namespace WebApp.Services.Users
{
    public interface IUserService
    {
        Task<BaseApiResult> CreateUser(RegistrationViewModel model);
        Task<UserIdentityModel> AuthenticateUser(LoginViewModel model);
    }
}
