using System.Threading.Tasks;
using WebApp.Models.User;

namespace WebApp.Services.User
{
    public interface IUserService
    {
        Task<UserIdentityModel> AuthenticateUser(LoginViewModel model);
    }
}
