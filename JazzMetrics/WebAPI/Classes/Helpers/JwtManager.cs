using Microsoft.IdentityModel.Tokens;
using WebAPI.Classes.Error;
using WebAPI.Models.Error;
using System;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebAPI.Classes.Helpers
{
    /// <summary>
    /// trida pro spravu JWT tokenu
    /// </summary>
    public static class JwtManager
    {
        /// <summary>
        /// vygenerovano pomoci ->
        ///     var hmac = new HMACSHA256();
        ///     var key = Convert.ToBase64String(hmac.Key);
        /// </summary>
        private const string Secret = "UzvmOvv5iwCQmpmUNugxCtUSD+wLRRNQoYCEN/faC53LOoIkkc+dzHjLZZSH/mAh6Rs69L2RLTqFWIes5qIIhA==";

        /// <summary>
        /// vygeneruje novy JWT token pro uzivatele
        /// </summary>
        /// <param name="username">uzivatelske jmeno uzivatele</param>
        /// <param name="workplace">filialka uzivatele</param>
        /// <param name="expireMinutes">token vyprsi za xx minut - default je 24hodin</param>
        /// <returns></returns>
        public static string GenerateToken(string username)
        {
            byte[] symmetricKey = Convert.FromBase64String(Secret);
            var tokenHandler = new JwtSecurityTokenHandler();

            int expireMinutes = Convert.ToInt32(ConfigurationManager.AppSettings["tokenExpirationTime"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, username),
                }),
                Expires = DateTime.UtcNow.AddMinutes(expireMinutes), //nutne zadat UTC time
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetricKey), SecurityAlgorithms.HmacSha256Signature)
            };

            return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
        }

        /// <summary>
        /// metoda, ktera validuje token, v pripade, ze je validni vrati principals, pokud ne, vrati null
        /// </summary>
        /// <param name="token">retezec reprezentujici string</param>
        /// <returns>principals uzivatele (jeho session)</returns>
        public static async Task<ClaimsPrincipal> GetPrincipal(string token)
        {
            try
            {
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                JwtSecurityToken jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

                if (jwtToken == null)
                {
                    return null;
                }

                byte[] symmetricKey = Convert.FromBase64String(Secret);

                //Logger.WriteToFile("JwtLog.txt", $"{jwtToken.ValidFrom} -- {jwtToken.ValidTo}"); //pro testovani funkcnosti JWT

                TokenValidationParameters validationParameters = new TokenValidationParameters()
                {
                    ClockSkew = TimeSpan.FromMinutes(3), //pro testovani je fajn nastavit TimeSpan.FromSeconds(1) -> jinak to defaultne bere casovou rezervu 5 minut
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(symmetricKey)
                };

                return tokenHandler.ValidateToken(token, validationParameters, out SecurityToken securityToken);
            }
            catch(SecurityTokenExpiredException) //kdyz vyprsi expiration time tokenu
            {
                return null;
            }
            catch (Exception e)
            {
                await ErrorManager.SaveErrorToDB(new ErrorModel(e, module: "JwtManager", function: "GetPrincipal"));

                return null;
            }
        }
    }
}