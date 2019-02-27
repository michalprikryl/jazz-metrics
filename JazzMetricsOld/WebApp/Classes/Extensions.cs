using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebApp
{
    public static class Extensions
    {
        /// <summary>
        /// 'dekoduje' retezec z non-human readable retezce (base64)
        /// </summary>
        /// <param name="base64EncodedData">string 'zakodovany' v base64</param>
        /// <returns></returns>
        public static string DecodeFromBase64(this string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }

        /// <summary>
        /// 'zakoduje' retezec do non-human readable retezce (base64)
        /// </summary>
        /// <param name="plainText">retezec, ktery chci zakodovat</param>
        /// <returns></returns>
        public static string EncodeToBase64(this string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        /// <summary>
        /// kontroluje, zda je dana stranka z menu zobrazena ci nikoli
        /// </summary>
        /// <param name="html"></param>
        /// <param name="controllers">controller, pro ktery se ma zobrazeni kontrolovat</param>
        /// <param name="actions">akce controlleru, pro kterou se ma zobrazeni kontrolovat</param>
        /// <param name="cssClass">css trida, ktera se ma vratit v pripade zobrazeni dane stranky (default je 'active')</param>
        /// <returns></returns>
        public static string IsSelected(this HtmlHelper html, string controllers = "", string actions = "", string cssClass = "active")
        {
            ViewContext viewContext = html.ViewContext;

            if (viewContext.Controller.ControllerContext.IsChildAction)
            {
                viewContext = html.ViewContext.ParentActionViewContext;
            }

            RouteValueDictionary routeValues = viewContext.RouteData.Values;
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

            return acceptedActions.Contains(currentAction) && acceptedControllers.Contains(currentController) ? cssClass : string.Empty;
        }

        /// <summary>
        /// vraci sirku sloupce (col-md-XX) dle urovne zanoreni komponenty
        /// </summary>
        /// <param name="html"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public static string GetPadding(this HtmlHelper html, int level)
        {
            if (level == 1)
            {
                return $"col-md-12";
            }
            else
            {
                return $"col-md-10";
            }
        }

        /// <summary>
        /// vraci css tridu (collapse), zda ma byt komponenta na dane urovni zobrazena - dale pokud ji mel uzivatel pred refreshem view rozbalenou, tak se rozbali taktez
        /// </summary>
        /// <param name="html"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public static string DisplayMenu(this HtmlHelper html, int level, string id, List<string> path)
        {
            if (level == 1)
            {
                return string.Empty;
            }
            else
            {
                if (path.Count >= level - 1 && path[level - 2] == id)
                {
                    return "collapse show";
                }
                else
                {
                    return "collapse";
                }
            }
        }
    }
}