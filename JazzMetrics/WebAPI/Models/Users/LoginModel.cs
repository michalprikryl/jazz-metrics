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

    /// <summary>
    /// navratovy objekt pri prihlaseni uzivatele
    /// </summary>
    public class LoginResponseModel : BaseResponseModel
    {
        /// <summary>
        /// JWT pro nasledne prihlaseni do API
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// user data
        /// </summary>
        public UserCookieModel User { get; set; }
    }

    /// <summary>
    /// trida pro kontext uzivatele ve scanneru
    /// </summary>
    public class UserCookieModel
    {
        /// <summary>
        /// ID uzivatele z DB
        /// </summary>
        public int UserId { get; set; }
        public string Username { get; set; }
        /// <summary>
        /// jmeno uzivatele
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// prijmeni uzivatele
        /// </summary>
        public string Lastname { get; set; }
        /// <summary>
        /// username uzivatele
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// role uzivatele
        /// </summary>
        public string Role { get; set; }
        public int? CompanyId { get; set; }
    }
}
