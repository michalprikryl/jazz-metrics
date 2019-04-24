using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Library
{
    /// <summary>
    /// rozsirujici metody
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Claim pro userId
        /// </summary>
        public const string UserIdClaim = "userid";

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

        /// <summary>
        /// vrati nazev role uzivatele z uzivatelskych udaju
        /// </summary>
        /// <param name="user">uzivatelsky kontext</param>
        /// <returns></returns>
        public static string GetUserRole(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;
        }

        /// <summary>
        /// vrati string v obracene podobe abc => cba
        /// </summary>
        /// <param name="text">string k obraceni</param>
        /// <returns></returns>
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

        /// <summary>
        /// extension pro vyhledani prvniho prvku v DB tabulce
        /// </summary>
        /// <typeparam name="T">typ entity z DB</typeparam>
        /// <param name="dbSet">tabulka z DB</param>
        /// <param name="expression">vyraz pro hledani prvku v tabulce</param>
        /// <param name="tracking">true -> pouzije se tracking (hlidani zmen hodnot properties)</param>
        /// <returns></returns>
        public static async Task<T> FirstOrDefaultAsyncSpecial<T>(this IQueryable<T> dbSet, Expression<Func<T, bool>> expression, bool tracking) where T : class
        {
            return await (tracking ? dbSet.FirstOrDefaultAsync(expression) : dbSet.AsNoTracking().FirstOrDefaultAsync(expression));
        }

        /// <summary>
        /// extension pro vyhledani prvniho prvku v DB tabulce, vyuziva Eager loading
        /// </summary>
        /// <typeparam name="T">typ entity z DB</typeparam>
        /// <typeparam name="U">typ navazne entity z DB</typeparam>
        /// <param name="dbSet">tabulka z DB</param>
        /// <param name="expression">vyraz pro hledani prvku v tabulce</param>
        /// <param name="tracking">true -> pouzije se tracking (hlidani zmen hodnot properties)</param>
        /// <param name="include">vyraz pro Eager loading</param>
        /// <returns></returns>
        public static async Task<T> FirstOrDefaultAsyncSpecial<T, U>(this IQueryable<T> dbSet, Expression<Func<T, bool>> expression, bool tracking, Expression<Func<T, U>> include) where T : class
        {
            if (include != null && tracking)
            {
                return await dbSet.Include(include).FirstOrDefaultAsync(expression);
            }
            else if (tracking)
            {
                return await dbSet.FirstOrDefaultAsync(expression);
            }
            else if (include != null)
            {
                return await dbSet.Include(include).AsNoTracking().FirstOrDefaultAsync(expression);
            }
            else
            {
                return await dbSet.AsNoTracking().FirstOrDefaultAsync(expression);
            }
        }

        /// <summary>
        /// specialni vyvolani stazeni dat z DB -> nehlida upravy properties stazenych objektu + pridava moznost Eager loadingu
        /// </summary>
        /// <typeparam name="T">typ entity z DB</typeparam>
        /// <typeparam name="U">typ navazne entity z DB</typeparam>
        /// <param name="dbSet">tabulka z DB</param>
        /// <param name="expression">vyraz pro hledani prvku v tabulce</param>
        /// <returns></returns>
        public static async Task<List<T>> ToListAsyncSpecial<T, U>(this IQueryable<T> dbSet, Expression<Func<T, U>> expression) where T : class
        {
            if (expression == null)
            {
                return await dbSet.AsNoTracking().ToListAsync();
            }
            else
            {
                return await dbSet.Include(expression).AsNoTracking().ToListAsync();
            }
        }

        /// <summary>
        /// specialni vyvolani stazeni dat z DB -> nehlida upravy properties stazenych objektu
        /// </summary>
        /// <typeparam name="T">typ entity z DB</typeparam>
        /// <param name="dbSet">tabulka z DB</param>
        /// <returns></returns>
        public static async Task<List<T>> ToListAsyncSpecial<T>(this IQueryable<T> dbSet) where T : class
        {
            return await dbSet.AsNoTracking().ToListAsync();
        }
    }
}
