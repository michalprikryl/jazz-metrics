using Database.DAO;
using Library.Models;
using Library.Models.User;
using Library.Models.Users;
using System.Threading.Tasks;
using WebAPI.Services.Helpers;

namespace WebAPI.Services.Users
{
    /// <summary>
    /// interface pro servis pro praci s DB tabulkou User
    /// </summary>
    public interface IUserService : ICrudOperations<UserModel, User>
    {
        /// <summary>
        /// autentizuje prihlasovaci udaje uzivatele
        /// </summary>
        /// <param name="model">prihlasovaci udaje</param>
        /// <returns></returns>
        Task<BaseResponseModelGet<UserIdentityModel>> CheckUser(LoginRequestModel model);
        /// <summary>
        /// vytvori JWT
        /// </summary>
        /// <param name="id">ID uzivatele</param>
        /// <param name="userRole">nazevv role uzivatele</param>
        /// <returns></returns>
        Task<string> BuildToken(int id, string userRole);
        /// <summary>
        /// vrati uzivatele dle jeho uzivatelskeho jmena
        /// </summary>
        /// <param name="username">uzivatelske jmeno hledaneho uzivatele</param>
        /// <returns></returns>
        Task<BaseResponseModelPost> GetByUsername(string username);
    }
}
