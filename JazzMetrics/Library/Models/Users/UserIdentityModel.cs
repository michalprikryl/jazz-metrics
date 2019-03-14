namespace Library.Models.User
{
    /// <summary>
    /// trida predstavujici navratovy objekt z API - pri identifikaci uzivatele
    /// </summary>
    public class UserIdentityModel
    {
        public UserCookieModel User { get; set; }
        public string Token { get; set; }
    }

    /// <summary>
    /// objekt samotneho uzivatele - popis z API
    /// </summary>
    public class UserCookieModel
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public int? CompanyId { get; set; }

        public override string ToString() => $"{Firstname} {Lastname} ({Username}), email - {Email}, with user role '{Role}'{(CompanyId.HasValue ? $", from company #{CompanyId}" : string.Empty)}";
    }
}
