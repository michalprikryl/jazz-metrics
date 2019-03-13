using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
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

                        AddViewModelToTempData(model);

                        return RedirectToAction("Login");
                    }
                    else
                    {
                        AddMessageToModel(model, resultCompany.Message);
                    }
                }
                else
                {
                    AddMessageToModel(model, "Password and cofirmed password are not equal!");
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
                try
                {
                    UserIdentityModel identity = await _userService.AuthenticateUser(model);
                    if (identity == null)
                    {
                        model.MessageList.Add(new Tuple<string, bool>("Server is not accessible.", true));
                    }
                    else if (identity.Success && identity.User != null && !string.IsNullOrEmpty(identity.Token))
                    {
                        Task login = UserLogin(identity.User, identity.Token);

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

                        model.MessageList.Add(new Tuple<string, bool>(identity.Message ?? "Incorrect username or password.", true));
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
                if (MyUser.CompanyId == response.CompanyId || User.IsInRole(RoleSuperAdmin))
                {
                    model.Id = response.Id;
                    model.Username = response.Username;
                    model.Firstname = response.Firstname;
                    model.Lastname = response.Lastname;
                    model.Email = response.Email;
                    model.Admin = response.Admin;
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
                    model.Id = response.Id;
                    model.Firstname = response.Firstname;
                    model.Lastname = response.Lastname;
                    model.Email = response.Email;
                    model.UseLdaplogin = response.UseLdaplogin;
                    model.LdapUrl = response.LdapUrl;
                    model.LanguageId = response.LanguageId.ToString();
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