using Database;
using Database.DAO;
using Library;
using Library.Models;
using Library.Models.User;
using Library.Models.Users;
using Library.Networking;
using Library.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WebAPI.Controllers;
using WebAPI.Services.Helper;
using WebAPI.Services.Helpers;
using WebAPI.Services.Setting;

namespace WebAPI.Services.Users
{
    public class UserService : BaseDatabase, IUserService
    {
        private readonly IConfiguration _config;
        private readonly IHelperService _helperService;
        private readonly ISettingService _settingService;

        public UserService(JazzMetricsContext db, ISettingService settingService, IConfiguration config, IHelperService helperService) : base(db)
        {
            _config = config;
            _helperService = helperService;
            _settingService = settingService;
        }

        public async Task<BaseResponseModelGetAll<UserModel>> GetAll(bool lazy)
        {
            var response = new BaseResponseModelGetAll<UserModel> { Values = new List<UserModel>() };

            foreach (var item in await Database.User.ToListAsync())
            {
                response.Values.Add(ConvertToModel(item));
            }

            return response;
        }

        public async Task<BaseResponseModelGet<UserModel>> Get(int id, bool lazy)
        {
            var response = new BaseResponseModelGet<UserModel>();

            User user = await Load(id, response);
            if (user != null)
            {
                response.Value = ConvertToModel(user);
            }

            return response;
        }

        public async Task<BaseResponseModelPost> Create(UserModel request)
        {
            BaseResponseModelPost response = new BaseResponseModelPost();

            if (request.Validate())
            {
                if (ValidateEmail(request.Email, response) && ValidatePassword(request.Password, response))
                {
                    request.Username = request.Username ?? request.Email;
                    if (await CheckUsername(request.Username, response) && await CheckLanguage(request.LanguageId, response))
                    {
                        User user = new User
                        {
                            Email = request.Email,
                            FirstName = request.Firstname,
                            LanguageId = request.LanguageId,
                            LastName = request.Lastname,
                            LdapUrl = request.LdapUrl,
                            Salt = PasswordHelper.GeneratePassword(10),
                            Username = request.Username,
                            UseLdaplogin = request.UseLdaplogin,
                            CompanyId = request.CompanyId
                        };

                        user.Password = PasswordHelper.EncodePassword(request.Password, user.Salt);

                        if (request.CompanyId.HasValue)
                        {
                            user.UserRole = await GetUserRole(MainController.RoleAdmin);
                        }
                        else
                        {
                            user.UserRole = await GetUserRole(MainController.RoleUser);
                        }

                        await Database.User.AddAsync(user);

                        await Database.SaveChangesAsync();

                        response.Id = user.Id;
                        response.Message = "User was successfully created!";
                    }
                }
            }
            else
            {
                response.Success = false;
                response.Message = "Some of the required user properties is not present!";
            }

            return response;
        }

        public async Task<BaseResponseModel> Edit(UserModel request)
        {
            BaseResponseModel response = new BaseResponseModel();

            if (request.ValidateEdit())
            {
                if (ValidateEmail(request.Email, response) && await CheckLanguage(request.LanguageId, response))
                {
                    User user = await Load(request.Id, response);
                    if (user != null)
                    {
                        user.FirstName = request.Firstname;
                        user.LastName = request.Lastname;
                        user.Email = request.Email;
                        user.UseLdaplogin = request.UseLdaplogin;
                        user.LdapUrl = request.UseLdaplogin ? request.LdapUrl : null;
                        user.LanguageId = request.LanguageId;

                        await Database.SaveChangesAsync();

                        response.Message = "User was successfully edited!";
                    }
                }
            }
            else
            {
                response.Success = false;
                response.Message = "Some of the required properties is not present!";
            }

            return response;
        }

        public async Task<BaseResponseModel> Drop(int id)
        {
            BaseResponseModel response = new BaseResponseModel();

            User user = await Load(id, response);
            if (user != null)
            {
                Database.ProjectUser.RemoveRange(user.ProjectUser);

                Database.User.Remove(user);

                await Database.SaveChangesAsync();

                response.Message = "User was successfully deleted!";
            }

            return response;
        }

        public async Task<BaseResponseModel> PartialEdit(int id, List<PatchModel> request)
        {
            BaseResponseModel response = new BaseResponseModel();

            User user = await Load(id, response);
            if (user != null)
            {
                bool save = true;
                foreach (var item in request)
                {
                    if (string.Compare(item.PropertyName, "companyId", true) == 0)
                    {
                        var newValue = int.TryParse(item.Value, out int i) ? i : default(int?);
                        if ((user.CompanyId.HasValue && !newValue.HasValue) || (!user.CompanyId.HasValue && newValue.HasValue))
                        {
                            user.CompanyId = newValue;
                        }
                        else if (user.CompanyId.HasValue && newValue.HasValue)
                        {
                            save = response.Success = false;
                            response.Message = $"User's company cannot be changed, because this user is already member of {(user.CompanyId.Value == newValue.Value ? "this" : "some")} company!";

                            break;
                        }
                    }
                    else if (string.Compare(item.PropertyName, "userRoleId", true) == 0)
                    {
                        if (user.UserRole.Name != MainController.RoleSuperAdmin)
                        {
                            user.UserRole = (!string.IsNullOrEmpty(item.Value) && item.Value == MainController.RoleUser) || user.UserRole.Name == MainController.RoleAdmin 
                                ? await GetUserRole(MainController.RoleUser) : await GetUserRole(MainController.RoleAdmin);
                        }
                    }
                    else if (string.Compare(item.PropertyName, "password", true) == 0)
                    {
                        string[] passwords = item.Value.Split(";;;");
                        if (passwords.Length == 2)
                        {
                            if (ComparePasswords(user, passwords[0]))
                            {
                                if (ValidatePassword(passwords[1], response))
                                {
                                    user.Password = PasswordHelper.EncodePassword(passwords[1], user.Salt);
                                }
                                else
                                {
                                    save = false;

                                    break;
                                }
                            }
                            else
                            {
                                save = response.Success = false;
                                response.Message = "User's old password doesn't match!";

                                break;
                            }
                        }
                    }
                }

                if (save)
                {
                    await Database.SaveChangesAsync();

                    response.Message = $"User was successfully edited!";
                }
            }

            return response;
        }

        public async Task<User> Load(int id, BaseResponseModel response)
        {
            User user = await Database.User.FirstOrDefaultAsync(a => a.Id == id);
            if (user == null)
            {
                response.Success = false;
                response.Message = "Unknown user!";
            }

            return user;
        }

        public UserModel ConvertToModel(User dbModel)
        {
            return new UserModel
            {
                Id = dbModel.Id,
                Firstname = dbModel.FirstName,
                Lastname = dbModel.LastName,
                Email = dbModel.Email,
                Username = dbModel.Username,
                UseLdaplogin = dbModel.UseLdaplogin,
                LdapUrl = dbModel.LdapUrl,
                LanguageId = dbModel.LanguageId,
                CompanyId = dbModel.CompanyId,
                UserRoleId = dbModel.UserRoleId,
                Admin = dbModel.UserRole.Name.Contains("admin", StringComparison.OrdinalIgnoreCase)
            };
        }

        public async Task<BaseResponseModelPost> GetByUsername(string username)
        {
            BaseResponseModelPost response = new BaseResponseModelPost();

            User user = await Database.User.FirstOrDefaultAsync(a => a.Username == username);
            if (user == null)
            {
                response.Success = false;
                response.Message = "Unknown username!";
            }
            else
            {
                response.Id = user.Id;
            }

            return response;
        }

        /// <summary>
        /// zkontroluje uzivatele
        /// </summary>
        /// <param name="model">prijate informace o uzivateli</param>
        /// <returns></returns>
        public async Task<BaseResponseModelGet<UserIdentityModel>> CheckUser(LoginRequestModel model)
        {
            var result = new BaseResponseModelGet<UserIdentityModel> { Value = new UserIdentityModel() };

            if (model.Validate())
            {
                model.Username = model.Username.Trim();
                model.Password = model.Password.Trim();

                User user = await Database.User.FirstOrDefaultAsync(u => u.Username == model.Username);
                if (user != null)
                {
                    if ((user.UseLdaplogin && CheckLdapLogin(user.LdapUrl, model.Username, model.Password)) || ComparePasswords(user, model.Password))
                    {
                        await CreateUserSession(user, result.Value);
                    }
                    else
                    {
                        result.Success = false;
                        result.Message = "Incorrect credentials!";
                    }
                }
                else
                {
                    result.Success = false;
                    result.Message = "Unknown credentials!";
                }
            }
            else
            {
                result.Success = false;
                result.Message = "Enter username and password!";
            }

            return result;
        }

        public async Task<string> BuildToken(int id)
        {
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            string setting = await _settingService.GetSettingValue("TokenExpiration", "TokenExpirationMinutes");

            if (string.IsNullOrEmpty(setting) || !int.TryParse(setting, out int minutes))
            {
                minutes = int.Parse(_config["Jwt:DefaultExp"]);
            }

            string issuer = _config["Jwt:Issuer"];

            var claims = new[]
            {
                new Claim(Extensions.UserIdClaim, id.ToString()),
            };

            var token = new JwtSecurityToken(issuer, issuer, claims, expires: DateTime.Now.AddMinutes(minutes), signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// kontrola prihlaseni pomoci LDAP
        /// </summary>
        /// <param name="url">url s ldap</param>
        /// <param name="user">uzivatelske jmeno</param>
        /// <param name="password">heslo</param>
        /// <returns>vysledek prihlaseni</returns>
        private bool CheckLdapLogin(string url, string user, string password)
        {
            bool login = true;
            ////url -> LDAP://edsldap.dhl.com:389/ou=users,o=dhl.com[uid] TODO
            //if (!string.IsNullOrEmpty(url))
            //{
            //    string adsiFullPath = url.Remove(url.IndexOf("[")); // LDAP://edsldap.dhl.com:389/ou=users,o=dhl.com

            //    string adsiPath = adsiFullPath.Substring(adsiFullPath.IndexOf(":") + 1); // //edsldap.dhl.com:389/ou=users,o=dhl.com
            //    if (adsiPath.IndexOf(":") > 0)
            //    {
            //        adsiPath = adsiPath.Substring(adsiPath.IndexOf("/", adsiPath.IndexOf(":") + 1) + 1); // ou=users,o=dhl.com
            //    }
            //    else
            //    {
            //        adsiPath = adsiPath.Substring(2); // edsldap.dhl.com:389/ou=users,o=dhl.com
            //    }

            //    string adsiProp = url.Substring(url.IndexOf("[") + 1, url.IndexOf("]") - url.IndexOf("[") - 1); // uid

            //    //DirectoryEntry dirEntry = new DirectoryEntry(adsiFullPath, $"{adsiProp}={user},{adsiPath}", password, AuthenticationTypes.None); // uid=gsd,ou=users,o=dhl.com

            //    try
            //    {
            //        //object obj = dirEntry.NativeObject;
            //        login = true;
            //    }
            //    catch (Exception e)
            //    {
            //        if (e.Message.Trim() != "The user name or password is incorrect.")
            //        {
            //            await _helperService.SaveErrorToDB(new ErrorModel(e, module: "LDAPLogin", function: "ADSIAuth", message: $"UID-{user};PASSWD-{password};URL-{url}"));
            //        }
            //    }
            //    finally
            //    {
            //        //dirEntry.Close();
            //    }
            //}

            return login;
        }

        private async Task CreateUserSession(User user, UserIdentityModel result)
        {
            result.Token = await BuildToken(user.Id);
            result.User = new UserCookieModel
            {
                Username = user.Username,
                Email = user.Email,
                Firstname = user.FirstName,
                Lastname = user.LastName,
                Role = user.UserRole.Name,
                UserId = user.Id,
                CompanyId = user.CompanyId
            };
        }

        private bool ComparePasswords(User user, string sentPasswd)
        {
            return user.Password == PasswordHelper.EncodePassword(sentPasswd, user.Salt);
        }

        private async Task<UserRole> GetUserRole(string name) => await Database.UserRole.FirstAsync(r => r.Name == name);

        private bool ValidatePassword(string password, BaseResponseModel response)
        {
            if (Regex.IsMatch(password, @"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{6,50}$"))
            {
                return true;
            }
            else
            {
                response.Success = false;
                response.Message = "Password does not meet minimal requirements - one or more uppar case character, one or more lower case character and one or more number!";

                return false;
            }
        }

        private bool ValidateEmail(string email, BaseResponseModel response)
        {
            if (new EmailAddressAttribute().IsValid(email))
            {
                return true;
            }
            else
            {
                response.Success = false;
                response.Message = "Chosen email address isn't valid!";

                return false;
            }
        }

        private async Task<bool> CheckUsername(string username, BaseResponseModel response)
        {
            if (await Database.User.AllAsync(u => u.Username != username))
            {
                return true;
            }
            else
            {
                response.Success = false;
                response.Message = "Chosen username was already chosen by other user!";

                return false;
            }
        }

        private async Task<bool> CheckLanguage(int languageId, BaseResponseModel response)
        {
            if (await Database.Language.AnyAsync(u => u.Id == languageId))
            {
                return true;
            }
            else
            {
                response.Success = false;
                response.Message = "Unknown chosen language!";

                return false;
            }
        }
    }
}