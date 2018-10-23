using WebApp.Identity;
using WebApp.Models;
using System;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class AppController : Controller
    {
        /// <summary>
        /// vrati nazev cookie pro JWT
        /// </summary>
        protected string TokenCookieName => "MenuAdminValue";

        /// <summary>
        /// vrati hodnotu JWT prihlaseneho uzivatele
        /// </summary>
        protected string TokenValue => Request.Cookies.Get(TokenCookieName).Value;

        /// <summary>
        /// uzivatel nacteny v http contextu
        /// </summary>
        protected virtual new CustomPrincipal User
        {
            get { return HttpContext.User as CustomPrincipal; }
        }

        /// <summary>
        /// ulozi errory z modelstate collection do messagelistu modelu
        /// </summary>
        /// <param name="model">model, do ktereho se to ma vlozit</param>
        protected void AddModelStateErrors(ViewModel model)
        {
            foreach (var value in ModelState.Values)
            {
                foreach (var error in value.Errors)
                {
                    model.MessageList.Add(new Tuple<string, bool>(error.ErrorMessage, true));
                }
            }
        }
    }
}