using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
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
            if (User == null)
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
                    UserIdentityModel identity = await _userService.AuthenticateUser(model);
                    if (identity == null)
                    {
                        model.MessageList.Add(new Tuple<string, bool>("Nastala chyba připojení k serveru.", true));
                    }
                    else if (identity.ProperUser && identity.User != null && !string.IsNullOrEmpty(identity.Token))
                    {
                        UserModel user = identity.User;
                        //CustomSerializeModel userModel = new CustomSerializeModel()
                        //{
                        //    UserId = user.UserId,
                        //    FirstName = user.Firstname,
                        //    LastName = user.Lastname,
                        //    Email = user.Email,
                        //    Roles = new string[] { user.Role }
                        //};

                        //int expireMinutes = int.Parse(ConfigurationManager.AppSettings["cookieExpirationTime"]);
                        //DateTime expiration = DateTime.Now.AddMinutes(expireMinutes);

                        //string userData = JsonConvert.SerializeObject(userModel);
                        //FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1, model.UserName, DateTime.Now, expiration, false, userData);

                        //HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(authTicket)) { Expires = expiration };
                        //Response.Cookies.Add(faCookie);

                        //HttpCookie tokenCookie = new HttpCookie(TokenCookieName, identity.Token) { HttpOnly = true, Expires = expiration };
                        //Response.Cookies.Add(tokenCookie);

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

                        model.MessageList.Add(new Tuple<string, bool>(identity.Message ?? "Nesprávné uživatelské jméno nebo heslo.", true));
                    }
                }
                catch (Exception e)
                {
                    model.MessageList.Add(new Tuple<string, bool>("Při přihlašování nastala chyba, zkuste to prosím znovu.", true));

                    //await ErrorService.CreateError(new ErrorModel(e, User?.UserId.ToString() ?? "WA", module: "UserController", function: "Login"));
                }
            }
            else
            {
                AddModelStateErrors(model);
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult Logout()
        {
            HttpContext.SignOutAsync();

            return RedirectToAction("Login");
        }
    }
}