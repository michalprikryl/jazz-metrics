using Library.Models.User;
using Library.Networking;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApp.Models;
using WebApp.Services.Error;

namespace WebApp.Controllers
{
    [Authorize]
    public class AppController : Controller
    {
        public IErrorService ErrorService { get; }

        public const string RoleUser = "user";
        public const string RoleAdmin = "admin";
        public const string RoleSuperAdmin = "super-admin";

        public static string TokenClaim => "JWToken";
        public static string LastNameClaim => "LastName";
        public static string UserIdClaim => "UserId";
        public static string CompanyIdClaim => "CompanyId";

        /// <summary>
        /// uzivatel nacteny v http contextu
        /// </summary>
        public UserCookieModel MyUser
        {
            get => User.GetIdentity();
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

        protected void AddMessageToModel(ViewModel model, string message, bool error = true) => model.MessageList.Add(new Tuple<string, bool>(message, error));

        protected PatchModel CreatePatchModel(string propertyName, string propertyValue) => new PatchModel { PropertyName = propertyName, Value = propertyValue };

        protected List<PatchModel> CreatePatchList(params PatchModel[] models) => models.ToList();

        protected void AddViewModelToTempData(ViewModel model) => TempData["ViewModel"] = JsonConvert.SerializeObject(model);

        protected RedirectToActionResult RedirectToActionWithId(ViewModel model, string action, string controller, int id)
        {
            AddViewModelToTempData(model);

            return RedirectToAction(action, controller, new { id });
        }

        protected void CheckTempData(ViewModel model)
        {
            if (TempData["ViewModel"] != null)
            {
                var source = JsonConvert.DeserializeObject<ViewModel>(TempData["ViewModel"] as string);
                model.MessageList = source.MessageList;
            }
        }

        protected async Task UserLogin(UserCookieModel user, string token)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.GivenName, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Firstname),
                new Claim(LastNameClaim, user.Lastname),
                new Claim(UserIdClaim, user.UserId.ToString()),
                new Claim(CompanyIdClaim, user.CompanyId.ToString()),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(TokenClaim, token)
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwt = handler.ReadToken(token) as JwtSecurityToken;

            AuthenticationProperties authProperties = new AuthenticationProperties
            {
                //IsPersistent = true, //pokud chci, aby cookie zustavala i dele nez session
                IssuedUtc = DateTime.UtcNow,
                ExpiresUtc = DateTime.SpecifyKind(jwt.ValidTo, DateTimeKind.Utc)
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
        }
    }
}