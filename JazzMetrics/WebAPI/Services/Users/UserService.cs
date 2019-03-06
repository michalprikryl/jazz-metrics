using Database;
using Database.DAO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WebAPI.Models;
using WebAPI.Models.Users;
using WebAPI.Services.Error;
using WebAPI.Services.Helpers;
using WebAPI.Services.Setting;

namespace WebAPI.Services.Users
{
    public class UserService : BaseDatabase, IUserService
    {
        private readonly IConfiguration _config;
        private readonly IErrorService _errorService;
        private readonly ISettingService _settingService;

        public UserService(JazzMetricsContext db, ISettingService settingService, IConfiguration config, IErrorService errorService) : base(db)
        {
            _config = config;
            _errorService = errorService;
            _settingService = settingService;
        }

        public async Task<BaseResponseModel> Registration(RegistrationRequestModel model)
        {
            BaseResponseModel response = new BaseResponseModel { Success = true };

            if (model.Validate)
            {
                if (new EmailAddressAttribute().IsValid(model.Email))
                {
                    if (ValidatePassword(model.Password))
                    {
                        model.Username = model.Username ?? model.Email;
                        if (await Database.User.AllAsync(u => u.Username != model.Username))
                        {
                            if (await Database.Language.AnyAsync(u => u.Id == model.Language))
                            {
                                User user = new User
                                {
                                    Email = model.Email,
                                    FirstName = model.Firstname,
                                    LanguageId = model.Language,
                                    LastName = model.Lastname,
                                    LdapUrl = model.LdapUrl,
                                    Salt = PasswordHelper.GeneratePassword(10),
                                    Username = model.Username,
                                    UseLdaplogin = model.LdapLogin,
                                    UserRole = await Database.UserRole.FirstAsync(r => r.Name == "admin")
                                };

                                user.Password = PasswordHelper.EncodePassword(model.Password, user.Salt);

                                await Database.User.AddAsync(user);

                                await Database.SaveChangesAsync();

                                response.Message = "User was successfully created!";
                            }
                            else
                            {
                                response.Success = false;
                                response.Message = "Unknown chosen language!";
                            }
                        }
                        else
                        {
                            response.Success = false;
                            response.Message = "Chosen username was already chosen by other user!";
                        }
                    }
                    else
                    {
                        response.Success = false;
                        response.Message = "Password does not meet minimal requirements - one or more uppar case character, one or more lower case character and one or more number!";
                    }
                }
                else
                {
                    response.Success = false;
                    response.Message = "Chosen email address isn't valid!";
                }
            }
            else
            {
                response.Success = false;
                response.Message = "Some of the required user properties is not present!";
            }

            return response;
        }

        /// <summary>
        /// zkontroluje uzivatele
        /// </summary>
        /// <param name="model">prijate informace o uzivateli</param>
        /// <returns></returns>
        public async Task<LoginResponseModel> CheckUser(LoginRequestModel model)
        {
            LoginResponseModel result = new LoginResponseModel();

            if (model.Validate)
            {
                model.Username = model.Username.Trim();
                model.Password = model.Password.Trim();

                User user = await Database.User.FirstOrDefaultAsync(u => u.Email == model.Username);
                if (user != null)
                {
                    if ((user.UseLdaplogin && CheckLdapLogin(user.LdapUrl, model.Username, model.Password)) || ComparePasswords(user, model.Password))
                    {
                        await CreateUserSession(user, result);
                    }
                    else
                    {
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

        public async Task<string> BuildToken(string username)
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
                new Claim(JwtRegisteredClaimNames.Email, username)
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
            //            await _errorService.SaveErrorToDB(new ErrorModel(e, module: "LDAPLogin", function: "ADSIAuth", message: $"UID-{user};PASSWD-{password};URL-{url}"));
            //        }
            //    }
            //    finally
            //    {
            //        //dirEntry.Close();
            //    }
            //}

            return login;
        }

        private async Task CreateUserSession(User user, LoginResponseModel result)
        {
            result.Success = true;
            result.Token = await BuildToken(user.Email);
            result.User = new UserModel
            {
                Email = user.Email,
                FirstName = user.FirstName,
                Lastname = user.LastName,
                RoleId = user.UserRoleId,
                Role = user.UserRole.Name,
                UserId = user.Id
            };
        }

        private bool ComparePasswords(User user, string sentPasswd)
        {
            return user.Password == PasswordHelper.EncodePassword(sentPasswd, user.Salt);
        }

        private bool ValidatePassword(string password)
        {
            if (Regex.IsMatch(password, @"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{6,50}$"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
