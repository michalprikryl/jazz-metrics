using Library.Models.User;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using WebApp.Controllers;

namespace WebApp
{
    public static class Extensions
    {
        /// <summary>
        /// kontroluje, zda je dana stranka z menu zobrazena ci nikoli
        /// </summary>
        /// <param name="html"></param>
        /// <param name="controllers">controller, pro ktery se ma zobrazeni kontrolovat</param>
        /// <param name="actions">akce controlleru, pro kterou se ma zobrazeni kontrolovat</param>
        /// <param name="cssClass">css trida, ktera se ma vratit v pripade zobrazeni dane stranky (default je 'active')</param>
        /// <returns></returns>
        public static string IsSelected(this IHtmlHelper html, string controllers = "", string actions = "", string cssClass = "active")
        {
            RouteValueDictionary routeValues = html.ViewContext.RouteData.Values;
            string currentAction = routeValues["action"].ToString();
            string currentController = routeValues["controller"].ToString();

            if (string.IsNullOrEmpty(actions))
            {
                actions = currentAction;
            }

            if (string.IsNullOrEmpty(controllers))
            {
                controllers = currentController;
            }

            var acceptedActions = actions.Trim().Split(',').Distinct();
            var acceptedControllers = controllers.Trim().Split(',').Distinct();

            return acceptedActions.Any(a => string.Compare(a, currentAction, true) == 0) && acceptedControllers.Any(c => string.Compare(c, currentController, true) == 0) ? cssClass : string.Empty;
        }

        public static string IsCollapsed(this IHtmlHelper html, string value)
        {
            return string.IsNullOrEmpty(value) ? "collapse" : string.Empty;
        }

        public static string IsCollapsed(this IHtmlHelper html, bool value)
        {
            return value ? string.Empty : "collapse";
        }

        /// <summary>
        /// vrati username z uzivatelskych udaju
        /// </summary>
        /// <param name="user">uzivatelsky kontext</param>
        /// <returns></returns>
        public static string GetName(this ClaimsPrincipal user)
        {
            string firstName = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value ?? string.Empty;
            string lastName = user.Claims.FirstOrDefault(c => c.Type == AppController.LastNameClaim)?.Value ?? string.Empty;

            return $"{firstName} {lastName}";
        }

        public static int? CompanyId(this ClaimsPrincipal user)
        {
            return int.TryParse(user.Claims.FirstOrDefault(c => c.Type == AppController.CompanyIdClaim)?.Value ?? string.Empty, out int result) ? result : default(int?);
        }

        public static int UserId(this ClaimsPrincipal user)
        {
            return int.TryParse(user.Claims.FirstOrDefault(c => c.Type == AppController.UserIdClaim)?.Value ?? string.Empty, out int result) ? result : 0;
        }

        public static string EnglishNumber(this decimal number)
        {
            return number.ToString().Replace(",", ".");
        }

        public static string EnglishDateString(this DateTime date)
        {
            return date.ToString("yyyy-MM-dd");
        }

        public static UserCookieModel GetIdentity(this ClaimsPrincipal user)
        {
            if (user.Identity.IsAuthenticated)
            {
                return new UserCookieModel
                {
                    Username = user.FindFirstValue(ClaimTypes.GivenName),
                    Email = user.FindFirstValue(ClaimTypes.Email),
                    Firstname = user.FindFirstValue(ClaimTypes.Name),
                    Lastname = user.FindFirstValue(AppController.LastNameClaim),
                    Role = user.FindFirstValue(ClaimTypes.Role),
                    UserId = int.Parse(user.FindFirstValue(AppController.UserIdClaim)),
                    CompanyId = int.TryParse(user.FindFirstValue(AppController.CompanyIdClaim), out int n) ? n : default(int?)
                };
            }
            else
            {
                return null;
            }
        }

        public static bool IsAdmin(this ClaimsPrincipal user)
        {
            return user.IsInRole(AppController.RoleAdmin) || user.IsInRole(AppController.RoleSuperAdmin);
        }
    }
}
