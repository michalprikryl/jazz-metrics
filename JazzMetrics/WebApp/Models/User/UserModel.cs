namespace WebApp.Models.User
{
    /// <summary>
    /// trida predstavujici navratovy objekt z API - pri identifikaci uzivatele
    /// </summary>
    public class UserIdentityModel : BaseApiResult
    {
        public UserCookiesModel User { get; set; }
        public string Token { get; set; }
    }

    /// <summary>
    /// objekt samotneho uzivatele - popis z API
    /// </summary>
    public class UserCookiesModel
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public int? CompanyId { get; set; }

        public string FullName { get => $"{Firstname} {Lastname}"; }

        public override string ToString() => $"{Firstname} {Lastname} ({Username}), email - {Email}, with user role '{Role}'{(CompanyId.HasValue ? $", from company #{CompanyId}" : string.Empty)}";        
    }

    public class UserModel : BaseApiResult
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public int LanguageId { get; set; }
        public int UserRoleId { get; set; }
        public int? CompanyId { get; set; }
        public bool UseLdaplogin { get; set; }
        public string LdapUrl { get; set; }
        public bool Admin { get; set; }

        public override string ToString() => $"{Firstname} {Lastname} ({Email})";
    }
}
