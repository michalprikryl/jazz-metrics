using Library.Models;
using Library.Models.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Models.User;
using WebApp.Services.Crud;
using WebApp.Services.Error;
using WebApp.Services.Language;
using WebApp.Services.Users;

namespace WebApp.Controllers
{
    [Route("User")]
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

        [HttpGet("Registration"), AllowAnonymous]
        public async Task<ActionResult> Registration()
        {
            RegistrationViewModel model = new RegistrationViewModel();

            await GetLanguages(model);

            return View(model);
        }

        [HttpPost("Registration"), AllowAnonymous]
        public async Task<ActionResult> Registration(RegistrationViewModel model)
        {
            Task task = GetLanguages(model);

            if (ModelState.IsValid)
            {
                if (model.Password == model.ConfirmPassword)
                {
                    BaseResponseModelPost result = await _crudService.Create(model, null, UserService.UserEntity);

                    AddMessageToModel(model, result.Message, !result.Success);

                    if (result.Success)
                    {
                        AddViewModelToTempData(model);

                        return RedirectToAction("Login");
                    }
                }
                else
                {
                    AddMessageToModel(model, "Password and confirmed password are not equal!");
                }
            }

            await Task.WhenAll(task);

            return View(model);
        }

        [HttpGet("Login"), AllowAnonymous]
        public ActionResult Login(string returnUrl = "")
        {
            if (MyUser == null)
            {
                LoginViewModel model = new LoginViewModel();
                ViewBag.ReturnUrl = returnUrl;

                CheckTempData(model);

                if (model.MessageList.Any(m => !m.Item2))
                {
                    AddMessageToModel(model, "Your account to Jazz Metrics is prepared!", false);
                }

                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost("Login"), AllowAnonymous]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl = "")
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.AuthenticateUser(model);
                if (result == null)
                {
                    model.MessageList.Add(new Tuple<string, bool>("Server is not accessible.", true));
                }
                else if (result.Success && result.Value.User != null && !string.IsNullOrEmpty(result.Value.Token))
                {
                    Task login = UserLogin(result.Value.User, result.Value.Token);

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

                    model.MessageList.Add(new Tuple<string, bool>(result.Message ?? "Incorrect username or password.", true));
                }
            }

            return View(model);
        }

        [HttpGet("Logout")]
        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login", "User");
        }

        [HttpGet("AccessDenied")]
        public ActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet("Detail/{id}")]
        public async Task<ActionResult> Detail(int id)
        {
            UserViewModel model = new UserViewModel();

            var response = await _crudService.Get<UserModel>(id, Token, UserService.UserEntity);
            if (response.Success)
            {
                if (MyUser.CompanyId == response.Value.CompanyId || User.IsInRole(RoleSuperAdmin))
                {
                    model.Id = response.Value.Id;
                    model.Username = response.Value.Username;
                    model.Firstname = response.Value.Firstname;
                    model.Lastname = response.Value.Lastname;
                    model.Email = response.Value.Email;
                    model.Admin = response.Value.Admin;
                }
                else
                {
                    model.CanView = false;
                    AddMessageToModel(model, "You cannot display this user, because this user belongs to different company!");
                }
            }
            else
            {
                AddMessageToModel(model, response.Message);
            }

            return View(model);
        }

        [HttpGet("Edit/{id}")]
        public async Task<ActionResult> Edit(int id)
        {
            UserWorkModel model = new UserWorkModel();

            if (MyUser.UserId == id || User.IsInRole(RoleSuperAdmin))
            {
                Task task = GetLanguages(model);

                var response = await _crudService.Get<UserModel>(id, Token, UserService.UserEntity);

                if (response.Success)
                {
                    model.Id = response.Value.Id;
                    model.Firstname = response.Value.Firstname;
                    model.Lastname = response.Value.Lastname;
                    model.Email = response.Value.Email;
                    model.UseLdaplogin = response.Value.UseLdaplogin;
                    model.LdapUrl = response.Value.LdapUrl;
                    model.LanguageId = response.Value.LanguageId.ToString();
                }
                else
                {
                    AddMessageToModel(model, response.Message);
                }

                await Task.WhenAll(task);
            }
            else
            {
                model.CanView = false;
                AddMessageToModel(model, "You cannot edit this user!");
            }

            return View(model);
        }

        [HttpPost("Edit/{id}")]
        public async Task<IActionResult> Edit(int id, UserWorkModel model)
        {
            Task select = GetLanguages(model);

            if (ModelState.IsValid)
            {
                Task userTask = _crudService.Edit(id, model, Token, UserService.UserEntity).ContinueWith(async r =>
                {
                    var result = r.Result;
                    if (result.Success)
                    {
                        var user = MyUser;
                        user.Firstname = model.Firstname;
                        user.Lastname = model.Lastname;
                        user.Email = model.Email;
                        await UserLogin(user, Token);
                    }

                    if (!model.UseLdaplogin)
                    {
                        model.LdapUrl = null;
                    }

                    AddMessageToModel(model, result.Message, !result.Success);
                });

                Task passwordTask = Task.CompletedTask;
                if (!string.IsNullOrEmpty(model.OldPassword) && !string.IsNullOrEmpty(model.Password) && !string.IsNullOrEmpty(model.ConfirmPassword))
                {
                    if (model.Password == model.ConfirmPassword)
                    {
                        passwordTask = _crudService.PartialEdit(id, CreatePatchList(CreatePatchModel("password", $"{model.OldPassword};;;{model.Password}")), Token, UserService.UserEntity).ContinueWith(r =>
                        {
                            var passwordResult = r.Result;
                            if (passwordResult.Success)
                            {
                                AddMessageToModel(model, "Password was changed!", false);
                            }
                            else
                            {
                                AddMessageToModel(model, passwordResult.Message);
                            }
                        });
                    }
                    else
                    {
                        AddMessageToModel(model, "Password and cofirmed password are not equal! Password wasn't changed.");
                    }
                }

                await Task.WhenAll(userTask, passwordTask);
            }
            else
            {
                AddModelStateErrors(model);
            }

            await Task.WhenAll(select);

            return View(model);
        }

        private async Task GetLanguages(UserBaseModel model)
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