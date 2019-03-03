namespace WebApp.Models.User
{
    /// <summary>
    /// trida predstavujici navratovy objekt z API - pri identifikaci uzivatele
    /// </summary>
    public class UserIdentityModel : BaseApiResult
    {
        public UserModel User { get; set; }
        public string Token { get; set; }
    }

    /// <summary>
    /// objekt samotneho uzivatele - popis z API
    /// </summary>
    public class UserModel
    {
        public int UserId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string FullName { get => $"{Firstname} {Lastname}"; }
    }
}
