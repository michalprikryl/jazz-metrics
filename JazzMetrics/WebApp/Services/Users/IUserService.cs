using System.Threading.Tasks;
using WebApp.Models.User;

namespace WebApp.Services.Users
{
    public interface IUserService
    {
        Task<UserIdentityModel> AuthenticateUser(LoginViewModel model);
    }
}
