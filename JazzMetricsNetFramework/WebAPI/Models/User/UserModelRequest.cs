using WebAPI.Classes.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace WebAPI.Models.User
{
    /// <summary>
    /// trida, ktera slouzi pro prijem pozadavku na overeni uzivatele
    /// </summary>
    public class UserModelRequest
    {
        /// <summary>
        /// uzivatelske jmeno uzivatele
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// heslo uzivatele ziskane zvenci
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// validuje uzivatele - jestli ma zadane vlastnosti
        /// </summary>
        public bool Validate
        {
            get
            {
                return !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password);
            }
        }

        /// <summary>
        /// kontruluje, zda se zaslany uzivatel shoduje s nejakym jiz vytvorenym uzivatelem
        /// </summary>
        /// <param name="users">kolekce uzivatelu z DB nebo souboru</param>
        /// <returns></returns>
        public bool Compare(List<UserModelFromFile> users)
        {
            UserModelFromFile user = users.FirstOrDefault(u => u.Username == Username);
            if (user != null)
            {
                return user.Password == PasswordHelper.EncodePassword(Password, user.Salt);
            }
            else
            {
                return false;
            }
        }
    }

    /// <summary>
    /// docasny objekt pro nacitani ze souboru
    /// </summary>
    public class UserModelFromFile : UserModelRequest
    {
        public string Salt { get; set; }
    }
}