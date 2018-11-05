using Newtonsoft.Json;
using WebAPI.Classes.Helpers;
using WebAPI.Models.User;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using File = System.IO.File;
using Database;

namespace WebAPI.Classes.UserWork
{
    /// <summary>
    /// trida, kde probiha kontrola uctu uzivatele
    /// </summary>
    public static class LoginCheck
    {
        private static readonly string _file = $"{Extensions.PATH}login.json";

        /// <summary>
        /// kontroluje uzivatele podle souboru
        /// </summary>
        /// <param name="model">prijate informace o uzivateli</param>
        /// <returns></returns>
        public static bool CheckByFile(UserModelRequest model)
        {
            return CompareCredentials(model, string.Join("", File.ReadAllLines(_file, Encoding.UTF8)));
        }

        /// <summary>
        /// ontroluje uzivatele podle souboru (asynchronni verze)
        /// </summary>
        /// <param name="model">prijate informace o uzivateli</param>
        /// <returns></returns>
        public static async Task<bool> CheckByFileAsync(UserModelRequest model)
        {
            string fileContent;
            using (StreamReader reader = File.OpenText(_file))
            {
                fileContent = await reader.ReadToEndAsync();
            }

            return CompareCredentials(model, fileContent);
        }

        /// <summary>
        /// zkontroluje uzivatele
        /// </summary>
        /// <param name="model">prijate informace o uzivateli</param>
        /// <returns></returns>
        public static async Task<UserContainerModel> CheckByDb(UserModelRequest model)
        {
            UserContainerModel result = new UserContainerModel();

            model.Username = model.Username.Trim();
            model.Password = model.Password.Trim();

            using (JazzMetricsEntities db = new JazzMetricsEntities())
            {
                User user = await db.Users.FirstOrDefaultAsync(u => u.Email == model.Username);
                if (user != null)
                {
                    if (ComparePasswords(user, model.Password))
                    {
                        CreateUserSession(user, result);
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

            return result;
        }

        private static void CreateUserSession(User user, UserContainerModel result)
        {
            result.ProperUser = true;
            result.User = new UserCreator().CreateUserModel(user);
            result.Token = JwtManager.GenerateToken(user.Email);
        }

        private static bool ComparePasswords(User user, string sentPasswd)
        {
            return user.Password == PasswordHelper.EncodePassword(sentPasswd, user.Salt);
        }

        private static bool CompareCredentials(UserModelRequest model, string fileContent)
        {
            return model.Compare(JsonConvert.DeserializeObject<List<UserModelFromFile>>(fileContent));
        }
    }
}