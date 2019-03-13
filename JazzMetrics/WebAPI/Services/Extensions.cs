using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace WebAPI
{
    /// <summary>
    /// rozsirujici metody
    /// </summary>
    public static class Extensions
    {
        public static string UserIdClaim = "userid";

        /// <summary>
        /// cesta do App_Data, obsahuje na konci \\
        /// </summary>
        public static readonly string PATH = $"{AppDomain.CurrentDomain.BaseDirectory}App_Data\\";

        /// <summary>
        /// lehce zpracuje exception do stringu, aby o nem bylo mozne ziskat nejake zakladni info
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        internal static string ParseException(this Exception e)
        {
            return e != null ? $"{e.GetType().Name}{Environment.NewLine}{e.Message}{Environment.NewLine}{e.InnerException?.Message ?? string.Empty}" : string.Empty;
        }

        /// <summary>
        /// vraci string formatu "yyyy-MM-dd HH:mm:ss"
        /// </summary>
        /// <param name="date">datum, ktere chci prevest na string</param>
        /// <returns></returns>
        internal static string GetDateTimeString(this DateTime date)
        {
            return date.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// vrati username z uzivatelskych udaju
        /// </summary>
        /// <param name="user">uzivatelsky kontext</param>
        /// <returns></returns>
        internal static int GetId(this ClaimsPrincipal user)
        {
            return int.TryParse(user.FindFirstValue(UserIdClaim), out int id) ? id : 0;
        }
    }
}
