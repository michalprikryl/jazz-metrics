using System;
using System.Globalization;
using System.Security.Claims;

namespace Library
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
        public static string ParseException(this Exception e)
        {
            return e != null ? $"{e.GetType().Name}{Environment.NewLine}{e.Message}{Environment.NewLine}{e.InnerException?.Message ?? string.Empty}" : string.Empty;
        }

        /// <summary>
        /// vraci string formatu "yyyy-MM-dd HH:mm:ss"
        /// </summary>
        /// <param name="date">datum, ktere chci prevest na string</param>
        /// <returns></returns>
        public static string GetDateTimeString(this DateTime date)
        {
            return date.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// vraci string formatu "MMM dd, yyyy hh:mm:ss tt"
        /// </summary>
        /// <param name="date">datum, ktere chci prevest na string</param>
        /// <returns></returns>
        public static string GetDateTimeStringLong(this DateTime date)
        {
            return date.ToString("MMM dd, yyyy hh:mm:ss tt", CultureInfo.GetCultureInfo("en"));
        }

        /// <summary>
        /// vrati username z uzivatelskych udaju
        /// </summary>
        /// <param name="user">uzivatelsky kontext</param>
        /// <returns></returns>
        public static int GetId(this ClaimsPrincipal user)
        {
            return int.TryParse(user.FindFirst(UserIdClaim)?.Value ?? "0", out int id) ? id : 0;
        }

        public static string Reverse(this string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            char[] array = text.ToCharArray();
            Array.Reverse(array);
            return new string(array);
        }
    }
}
