using WebApp.Models.User;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace WebApp.Classes.User
{
    /// <summary>
    /// trida slouzi pro overeni identity uzivatele, kdy, pokud je overen, vrati i informace o nem
    /// </summary>
    public class CheckIdentity : ClientAPI
    {
        /// <summary>
        /// nutne zadat controller, na ktery se chci dotazovat
        /// </summary>
        /// <param name="controller"></param>
        public CheckIdentity(string controller = "user", bool useAuthetificationHeader = true, string jwt = null) : base(controller, useAuthetificationHeader, jwt) { }

        /// <summary>
        /// ziska uzivatele z API, kde jsou i overeny jeho prihlasovaci udaje
        /// </summary>
        /// <param name="model">data, ktere chci poslat na API, jejich vlastnosti musi odpovidat objektu z API</param>
        /// <returns></returns>
        public async Task<IdentityAPI> CheckInAPI(LoginViewModel model)
        {
            IdentityAPI result = new IdentityAPI();

            await PostToAPI(SerializeObjectToJSON(model), (task) =>
            {
                result = JsonConvert.DeserializeObject<IdentityAPI>(task.Result.Content.ReadAsStringAsync().Result);
            });

            return result;
        }
    }
}