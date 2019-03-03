namespace WebAPI.Models.Users
{
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
        public UserModel User { get; set; }
    }
}
