using Library.Networking;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using WebApp.Models;
using WebApp.Models.User;
using WebApp.Services.Error;

namespace WebApp.Controllers
{
    [Authorize]
    public class AppController : Controller
    {
        public IErrorService ErrorService { get; }

        public static string TokenClaim => "JWToken";
        public static string LastNameClaim => "LastName";
        public static string UserIdClaim => "UserId";
        public static string CompanyIdClaim => "CompanyId";

        /// <summary>
        /// uzivatel nacteny v http contextu
        /// </summary>
        public UserCookiesModel MyUser
        {
            get
            {
                if (User.Identity.IsAuthenticated)
                {
                    return new UserCookiesModel
                    {
                        Email = User.FindFirstValue(ClaimTypes.Email),
                        Firstname = User.FindFirstValue(ClaimTypes.Name),
                        Lastname = User.FindFirstValue(LastNameClaim),
                        Role = User.FindFirstValue(ClaimTypes.Role),
                        UserId = int.Parse(User.FindFirstValue(UserIdClaim)),
                        CompanyId = int.TryParse(User.FindFirstValue(CompanyIdClaim), out int n) ? n : default(int?)
                    };
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// vrati JWT z cookie
        /// </summary>
        public string Token
        {
            get => User.Identity.IsAuthenticated ? User.FindFirstValue(TokenClaim) : null;
        }

        public AppController(IErrorService errorService)
        {
            ErrorService = errorService;
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

        protected void AddMessageToModel(ViewModel model, string message, bool error = true)
        {
            model.MessageList.Add(new Tuple<string, bool>(message, error));
        }

        protected List<PatchModel> CreatePatchModel(string propertyName, string propertyValue)
        {
            return new List<PatchModel>
           {
               new PatchModel
               {
                   PropertyName = propertyName,
                   Value = propertyValue
               }
           };
        }

        protected void AddViewModelToTempData(ViewModel model)
        {
            TempData["ViewModel"] = JsonConvert.SerializeObject(model);
        }

        protected void CheckTempData(ViewModel model)
        {
            if (TempData["ViewModel"] != null)
            {
                var source = JsonConvert.DeserializeObject<ViewModel>(TempData["ViewModel"] as string);
                model.MessageList = source.MessageList;
            }
        }
    }
}