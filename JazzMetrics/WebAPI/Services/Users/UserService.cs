using Database;
using Database.DAO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Models.Error;
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
                    if ((user.UseLdaplogin && await CheckLdapLogin(user.LdapUrl, model.Username, model.Password)) || ComparePasswords(user, model.Password))
                    {
                        await CreateUserSession(user, result);
                    }
                    else
                    {
                        result.Message = "Nesprávné přihlašovací údaje!";
                    }
                }
                else
                {
                    result.ProperUser = false;
                    result.Message = "Neznámé přihlašovací jméno!";
                }
            }
            else
            {
                result.ProperUser = false;
                result.Message = "Zadejte přihlašovací jméno a heslo!";
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
                minutes = 1440;
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
        private async Task<bool> CheckLdapLogin(string url, string user, string password)
        {
            bool login = false;
            //url -> LDAP://edsldap.dhl.com:389/ou=users,o=dhl.com[uid] TODO
            if (!string.IsNullOrEmpty(url))
            {
                string adsiFullPath = url.Remove(url.IndexOf("[")); // LDAP://edsldap.dhl.com:389/ou=users,o=dhl.com

                string adsiPath = adsiFullPath.Substring(adsiFullPath.IndexOf(":") + 1); // //edsldap.dhl.com:389/ou=users,o=dhl.com
                if (adsiPath.IndexOf(":") > 0)
                {
                    adsiPath = adsiPath.Substring(adsiPath.IndexOf("/", adsiPath.IndexOf(":") + 1) + 1); // ou=users,o=dhl.com
                }
                else
                {
                    adsiPath = adsiPath.Substring(2); // edsldap.dhl.com:389/ou=users,o=dhl.com
                }

                string adsiProp = url.Substring(url.IndexOf("[") + 1, url.IndexOf("]") - url.IndexOf("[") - 1); // uid

                //DirectoryEntry dirEntry = new DirectoryEntry(adsiFullPath, $"{adsiProp}={user},{adsiPath}", password, AuthenticationTypes.None); // uid=gsd,ou=users,o=dhl.com

                try
                {
                    //object obj = dirEntry.NativeObject;
                    login = true;
                }
                catch (Exception e)
                {
                    if (e.Message.Trim() != "The user name or password is incorrect.")
                    {
                        await _errorService.SaveErrorToDB(new ErrorModel(e, module: "LDAPLogin", function: "ADSIAuth", message: $"UID-{user};PASSWD-{password};URL-{url}"));
                    }
                }
                finally
                {
                    //dirEntry.Close();
                }
            }

            return login;
        }

        private async Task CreateUserSession(User user, LoginResponseModel result)
        {
            result.ProperUser = true;
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
    }
}
