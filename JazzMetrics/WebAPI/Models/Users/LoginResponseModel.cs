namespace WebAPI.Models.Users
{
    /// <summary>
    /// navratovy objekt pri prihlaseni uzivatele
    /// </summary>
    public class LoginResponseModel
    {
        /// <summary>
        /// jestli je uzivatel prihlasen - je validni
        /// </summary>
        public bool ProperUser { get; set; }
        /// <summary>
        /// zprava pro uzivatele, pouze pri chybe
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// JWT pro nasledne prihlaseni do API
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// user data
        /// </summary>
        public UserModel User { get; set; }
    }
}
