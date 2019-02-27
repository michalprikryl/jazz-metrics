namespace WebAPI.Models.Users
{
    /// <summary>
    /// trida, ktera slouzi pro prijem pozadavku na overeni uzivatele
    /// </summary>
    public class LoginRequestModel
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
    }
}
