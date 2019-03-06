using System.Threading.Tasks;
using WebAPI.Models;
using WebAPI.Models.Users;

namespace WebAPI.Services.Users
{
    public interface IUserService
    {
        Task<BaseResponseModel> Registration(RegistrationRequestModel model);
        Task<LoginResponseModel> CheckUser(LoginRequestModel model);
        Task<string> BuildToken(string username);
    }
}
