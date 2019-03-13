using Library.Networking;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Threading.Tasks;
using WebApp.Models;
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
        public async Task<UserIdentityModel> AuthenticateUser(LoginViewModel model)
        {
            UserIdentityModel result = new UserIdentityModel();

            await PostToAPI(SerializeObjectToJSON(model), async httpResult =>
            {
                result = JsonConvert.DeserializeObject<UserIdentityModel>(await httpResult.Content.ReadAsStringAsync());
            }, "login");

            return result;
        }

        public async Task<BaseApiResultPost> FindUserIdByUsername(string username, string jwt)
        {
            BaseApiResultPost result = new BaseApiResultPost();

            await GetToAPI(GetParametersList(GetParameter("username", username)), async httpResult =>
            {
                result = JsonConvert.DeserializeObject<BaseApiResultPost>(await httpResult.Content.ReadAsStringAsync());
            }, jwt: jwt);

            return result;
        }
    }
}
