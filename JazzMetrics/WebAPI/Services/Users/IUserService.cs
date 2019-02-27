using System.Threading.Tasks;
using WebAPI.Models.Users;

namespace WebAPI.Services.Users
{
    public interface IUserService
    {
        Task<LoginResponseModel> CheckUser(LoginRequestModel model);
        Task<string> BuildToken(string username);
    }
}
