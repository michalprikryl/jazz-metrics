using WebAPI.Classes.Database;
using WebAPI.Classes.Helpers;
using System.Collections.Generic;
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
        private string _username;

        /// <summary>
        /// vrati user principals
        /// </summary>
        /// <param name="token">JWT token, ze ktereho se ziska kontext</param>
        /// <returns></returns>
        public async Task<IPrincipal> CreatePrincipal(string token)
        {
            if (await ValidateToken(token))
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, _username)
                };

                return new ClaimsPrincipal(new ClaimsIdentity(claims, "Jwt"));
            }

            return null;
        }

        /// <summary>
        /// samotna metoda pro validaci tokenu
        /// </summary>
        /// <param name="token">JWT token urceny k validaci</param>
        /// <returns></returns>
        private async Task<bool> ValidateToken(string token)
        {
            ClaimsPrincipal simplePrinciple = await JwtManager.GetPrincipal(token);
            ClaimsIdentity identity = simplePrinciple?.Identity as ClaimsIdentity;

            if (identity == null || !identity.IsAuthenticated)
            {
                return false;
            }

            _username = identity.GetUsername();

            if (string.IsNullOrEmpty(_username))
            {
                return false;
            }

            using (DatabaseContext = new JazzMetricsEntities())
            {
                return await DatabaseContext.Users.FirstOrDefaultAsync(p => p.Email == _username) != null;
            }
        }
    }
}