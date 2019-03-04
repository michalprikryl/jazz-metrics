using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApp.Models.Error;
using WebApp.Models.User;
using WebApp.Services.Error;
using WebApp.Services.User;

namespace WebApp.Controllers
{
    public class UserController : AppController
    {
        private readonly IUserService _userService;

        public UserController(IErrorService errorService, IUserService userService) : base(errorService) => _userService = userService;

        [HttpGet]
        [AllowAnonymous]
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

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
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
                        UserModel user = userIdentity.User;

                        List<Claim> claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Email, user.Email),
                            new Claim(ClaimTypes.Name, user.Firstname),
                            new Claim(LastNameClaim, user.Lastname),
                            new Claim(UserIdClaim, user.UserId.ToString()),
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
    }
}