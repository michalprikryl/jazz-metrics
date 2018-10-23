using System.Security.Principal;
using System.Threading.Tasks;
using WebAPI.Classes.UserWork;
using WebAPI.Models.User;

namespace WebAPI.Authentification
{
    /// <summary>
    /// trida pro kontrolu prihlasovacich udaju na API
    /// </summary>
    public class PrincipalProvider
    {
        /// <summary>
        /// autentifikace pomoci udaju zadanych v souboru
        /// </summary>
        /// <param name="username">uzivatelske jmeno</param>
        /// <param name="password">heslo</param>
        /// <returns></returns>
        public async Task<IPrincipal> CreatePrincipal(string username, string password)
        {
            if (await LoginCheck.CheckByFileAsync(new UserModelRequest { Username = username, Password = password }))
            {
                return new GenericPrincipal(new GenericIdentity(username), new[] { "User" });
            }
            else
            {
                return null;
            }
        }
    }
}