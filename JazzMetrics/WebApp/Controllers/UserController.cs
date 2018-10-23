using WebApp.Classes.Error;
using WebApp.Classes.User;
using WebApp.Identity;
using WebApp.Models.User;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace WebApp.Controllers
{
    public class UserController : AppController
    {
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
                    CheckIdentity check = new CheckIdentity();
                    IdentityAPI identity = await check.CheckInAPI(model);
                    if (identity == null)
                    {
                        model.MessageList.Add(new Tuple<string, bool>("Nastala chyba připojení k serveru.", true));
                    }
                    else if (identity.ProperUser && identity.User != null && !string.IsNullOrEmpty(identity.Token))
                    {
                        UserAPI user = identity.User;
                        CustomSerializeModel userModel = new CustomSerializeModel()
                        {
                            UserId = user.UserId,
                            FirstName = user.Firstname,
                            LastName = user.Lastname,
                            Email = user.Email,
                            Roles = new string[] { user.Role }
                        };

                        int expireMinutes = Convert.ToInt32(ConfigurationManager.AppSettings["cookieExpirationTime"]);
                        DateTime expiration = DateTime.Now.AddMinutes(expireMinutes);

                        string userData = JsonConvert.SerializeObject(userModel);
                        FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1, model.UserName, DateTime.Now, expiration, false, userData);

                        HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(authTicket)) { Expires = expiration };
                        Response.Cookies.Add(faCookie);

                        HttpCookie tokenCookie = new HttpCookie(TokenCookieName, identity.Token) { HttpOnly = true, Expires = expiration };
                        Response.Cookies.Add(tokenCookie);

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

                    ErrorSender sender = new ErrorSender();
                    await sender.CreateError(new ErrorAPI(e, User?.UserId.ToString() ?? "WA", module: "UserController", function: "Login"));
                }
            }
            else
            {
                AddModelStateErrors(model);
            }

            return View(model);
        }

        public ActionResult Logout()
        {
            Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, "")
            {
                Expires = DateTime.Now.AddYears(-1)
            });

            Response.Cookies.Add(new HttpCookie(TokenCookieName, "")
            {
                Expires = DateTime.Now.AddYears(-1)
            });

            FormsAuthentication.SignOut();

            return RedirectToAction("Login");
        }
    }
}