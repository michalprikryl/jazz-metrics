namespace WebApp.Models.User
{
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
