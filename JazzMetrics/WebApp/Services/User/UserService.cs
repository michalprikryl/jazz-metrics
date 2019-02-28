using Library.Networking;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Threading.Tasks;
using WebApp.Models.User;

namespace WebApp.Services.User
{
    public class UserService : ClientApiWithConfig, IUserService
    {
        public UserService(IConfiguration config, string controller = "user", string jwt = null) : base(config, controller, jwt) { }

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
            });

            return result;
        }
    }
}
