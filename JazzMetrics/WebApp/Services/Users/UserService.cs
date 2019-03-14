using Library.Models;
using Library.Models.User;
using Library.Networking;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Threading.Tasks;
using WebApp.Models.User;

namespace WebApp.Services.Users
{
    public class UserService : ClientApi, IUserService
    {
        public const string UserEntity = "user";

        public UserService(IConfiguration config) : base(config, UserEntity) { }

        /// <summary>
        /// ziska uzivatele z API, kde jsou i overeny jeho prihlasovaci udaje
        /// </summary>
        /// <param name="model">data, ktere chci poslat na API, jejich vlastnosti musi odpovidat objektu z API</param>
        /// <returns></returns>
        public async Task<BaseResponseModelGet<UserIdentityModel>> AuthenticateUser(LoginViewModel model)
        {
            var result = new BaseResponseModelGet<UserIdentityModel>();

            await PostToAPI(SerializeObjectToJSON(model), async httpResult =>
            {
                result = JsonConvert.DeserializeObject<BaseResponseModelGet<UserIdentityModel>>(await httpResult.Content.ReadAsStringAsync());
            }, "login");

            return result;
        }

        public async Task<BaseResponseModelPost> FindUserIdByUsername(string username, string jwt)
        {
            BaseResponseModelPost result = new BaseResponseModelPost();

            await GetToAPI(GetParametersList(GetParameter("username", username)), async httpResult =>
            {
                result = JsonConvert.DeserializeObject<BaseResponseModelPost>(await httpResult.Content.ReadAsStringAsync());
            }, jwt: jwt);

            return result;
        }
    }
}
