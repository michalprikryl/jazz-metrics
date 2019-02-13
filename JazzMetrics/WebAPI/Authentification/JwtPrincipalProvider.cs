using WebAPI.Classes.Database;
using WebAPI.Classes.Helpers;
using System.Data.Entity;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Database;

namespace WebAPI.Authentification
{
    /// <summary>
    /// trida pro vytvareni user principals pomoci JWT tokenu
    /// </summary>
    public class JwtPrincipalProvider : BaseDatabase
    {
        /// <summary>
        /// vrati user principals
        /// </summary>
        /// <param name="token">JWT token, ze ktereho se ziska kontext</param>
        /// <returns></returns>
        public async Task<IPrincipal> CreatePrincipal(string token)
        {
            return await ValidateToken(token);
        }

        /// <summary>
        /// samotna metoda pro validaci tokenu
        /// </summary>
        /// <param name="token">JWT token urceny k validaci</param>
        /// <returns></returns>
        private async Task<ClaimsPrincipal> ValidateToken(string token)
        {
            ClaimsPrincipal simplePrinciple = await JwtManager.GetPrincipal(token);
            if (!(simplePrinciple?.Identity is ClaimsIdentity identity) || !identity.IsAuthenticated)
            {
                return null;
            }

            string username = identity.GetUsername();
            if (string.IsNullOrEmpty(username))
            {
                return null;
            }

            bool correct = false;
            using (DatabaseContext = new JazzMetricsEntities())
            {
                correct = await DatabaseContext.Users.FirstOrDefaultAsync(p => p.Email == username) != null;
            }

            return correct ? simplePrinciple : null;
        }
    }
}