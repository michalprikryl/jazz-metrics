namespace Library.Models.Users
{
    public class UserModel
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

        public string Password { get; set; }

        public bool Validate() => !string.IsNullOrEmpty(Password) && ValidateEdit();

        public bool ValidateEdit() => !string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(Firstname) && !string.IsNullOrEmpty(Lastname) && (!UseLdaplogin || !string.IsNullOrEmpty(LdapUrl));

        public override string ToString() => $"{Firstname} {Lastname} ({Email})";
    }
}
