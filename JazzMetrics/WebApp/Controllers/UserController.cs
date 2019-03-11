using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApp.Models;
using WebApp.Models.Error;
using WebApp.Models.Setting.Company;
using WebApp.Models.User;
using WebApp.Services.Crud;
using WebApp.Services.Error;
using WebApp.Services.Language;
using WebApp.Services.Setting;
using WebApp.Services.Users;

namespace WebApp.Controllers
{
    public class UserController : AppController
    {
        private readonly IUserService _userService;
        private readonly ICrudService _crudService;
        private readonly ILanguageService _languageService;

        public UserController(IErrorService errorService, IUserService userService, ILanguageService languageService, ICrudService crudService) : base(errorService)
        {
            _userService = userService;
            _crudService = crudService;
            _languageService = languageService;
        }

        [HttpGet, AllowAnonymous]
        public async Task<ActionResult> Registration()
        {
            RegistrationViewModel model = new RegistrationViewModel();

            await GetLanguages(model);

            return View(model);
        }

        [HttpPost, AllowAnonymous]
        public async Task<ActionResult> Registration(RegistrationViewModel model)
        {
            Task task = GetLanguages(model);

            if (ModelState.IsValid)
            {
                BaseApiResultPost resultCompany = null;
                if (!string.IsNullOrEmpty(model.Company))
                {
                    resultCompany = await _crudService.Create(new CompanyModel { Name = model.Company }, null, SettingService.CompanyEntity);
                    model.CompanyId = resultCompany.Id;
                }

                if (resultCompany == null || resultCompany.Success)
                {
                    BaseApiResultPost result = await _crudService.Create(model, null, UserService.UserEntity);

                    if (!result.Success && resultCompany != null)
                    {
                        await _crudService.Drop(model.CompanyId.Value, null, SettingService.CompanyEntity);
                    }

                    AddMessageToModel(model, result.Message, !result.Success);
                }
                else
                {
                    AddMessageToModel(model, resultCompany.Message);
                }
            }

            await Task.WhenAll(task);

            return View(model);
        }

        [HttpGet, AllowAnonymous]
        public ActionResult Login(string returnUrl = "")
        {
            if (MyUser == null)
            {
                LoginViewModel model = new LoginViewModel();
                ViewBag.ReturnUrl = returnUrl;

                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost, AllowAnonymous]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl = "")
        {
            if (ModelState.IsValid)
            {
                try
                {
                    UserIdentityModel userIdentity = await _userService.AuthenticateUser(model);
                    if (userIdentity == null)
                    {
                        model.MessageList.Add(new Tuple<string, bool>("Server is not accessible.", true));
                    }
                    else if (userIdentity.Success && userIdentity.User != null && !string.IsNullOrEmpty(userIdentity.Token))
                    {
                        UserCookiesModel user = userIdentity.User;

                        List<Claim> claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Email, user.Email),
                            new Claim(ClaimTypes.Name, user.Firstname),
                            new Claim(LastNameClaim, user.Lastname),
                            new Claim(UserIdClaim, user.UserId.ToString()),
                            new Claim(CompanyIdClaim, user.CompanyId.ToString()),
                            new Claim(ClaimTypes.Role, user.Role),
                            new Claim(TokenClaim, userIdentity.Token)
                        };

                        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                        JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                        JwtSecurityToken token = handler.ReadToken(userIdentity.Token) as JwtSecurityToken;

                        AuthenticationProperties authProperties = new AuthenticationProperties
                        {
                            IsPersistent = true,
                            IssuedUtc = DateTime.UtcNow,
                            ExpiresUtc = DateTime.SpecifyKind(token.ValidTo, DateTimeKind.Utc)
                        };

                        var login = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                        if (Url.IsLocalUrl(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        ModelState.Remove("Password");

                        model.MessageList.Add(new Tuple<string, bool>(userIdentity.Message ?? "Incorrect username or password.", true));
                    }
                }
                catch (Exception e)
                {
                    model.MessageList.Add(new Tuple<string, bool>("Error occured during authorization, please try again.", true));

                    await ErrorService.CreateError(new ErrorModel(e, MyUser?.UserId.ToString() ?? "WA", module: "UserController", function: "Login"));
                }
            }

            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login", "User");
        }

        [HttpGet]
        public ActionResult AccessDenied()
        {
            return View();
        }

        private async Task GetLanguages(RegistrationViewModel model)
        {
            model.Languages = await _languageService.GetLanguagesForSelect();

            if (model.Languages == null || model.Languages.Count == 0)
            {
                AddMessageToModel(model, "Cannot retrieve languages, press F5 please.");
            }
            else
            {
                model.Languages.Last().Selected = true;
            }
        }
    }
}