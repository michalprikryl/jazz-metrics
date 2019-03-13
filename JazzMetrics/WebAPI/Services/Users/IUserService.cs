using Database.DAO;
using System.Threading.Tasks;
using WebAPI.Models;
using WebAPI.Models.Users;
using WebAPI.Services.Helpers;

namespace WebAPI.Services.Users
{
    public interface IUserService : ICrudOperations<UserModel, User>
    {
        Task<LoginResponseModel> CheckUser(LoginRequestModel model);
        Task<string> BuildToken(string username);
        Task<BaseResponseModelPost> GetByUsername(string username);
    }
}
