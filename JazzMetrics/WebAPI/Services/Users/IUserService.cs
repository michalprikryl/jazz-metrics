using Database.DAO;
using Library.Models;
using Library.Models.User;
using Library.Models.Users;
using System.Threading.Tasks;
using WebAPI.Services.Helpers;

namespace WebAPI.Services.Users
{
    public interface IUserService : ICrudOperations<UserModel, User>
    {
        Task<BaseResponseModelGet<UserIdentityModel>> CheckUser(LoginRequestModel model);
        Task<string> BuildToken(int id);
        Task<BaseResponseModelPost> GetByUsername(string username);
    }
}
