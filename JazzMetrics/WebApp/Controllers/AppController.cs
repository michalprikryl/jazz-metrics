using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using WebApp.Models;
using WebApp.Models.User;
using WebApp.Services.Error;
using WebApp.Services.User;

namespace WebApp.Controllers
{
    [Authorize]
    public class AppController : Controller
    {
        public IErrorService ErrorService { get; }
        public IUserManager UserManager { get; }

        /// <summary>
        /// uzivatel nacteny v http contextu
        /// </summary>
        public UserModel MyUser
        {
            get
            {
                string userEmail = User.FindFirstValue(ClaimTypes.Email);
                if (userEmail != null)
                {
                    return UserManager.GetUser(userEmail);
                }
                else
                {
                    return null;
                }
            }
        }

        public AppController(IErrorService errorService, IUserManager userManager)
        {
            ErrorService = errorService;
            UserManager = userManager;
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