using System;
using System.Security.Claims;

namespace WebAPI
{
    public static class Extensions
    {
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
            return $"{e.GetType().Name}\n{e.Message}\n{e.InnerException?.Message ?? string.Empty}";
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
        /// <param name="principal">uzivatelsky kontext</param>
        /// <returns></returns>
        internal static string GetUsername(this ClaimsIdentity principal)
        {
            return principal.FindFirst(ClaimTypes.Name)?.Value ?? null;
        }
    }
}