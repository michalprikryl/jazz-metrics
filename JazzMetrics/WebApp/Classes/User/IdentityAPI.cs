namespace WebApp.Classes.User
{
    /// <summary>
    /// trida predstavujici navratovy objekt z API - pri identifikaci uzivatele
    /// </summary>
    public class IdentityAPI
    {
        public bool ProperUser { get; set; }
        public UserAPI User { get; set; }
        public string Token { get; set; }
        public string Message { get; set; }
    }

    /// <summary>
    /// objekt samotneho uzivatele - popis z API
    /// </summary>
    public class UserAPI
    {
        public int UserId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}