namespace WebAPI.Models.Users
{
    public class RegistrationRequestModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public int Language { get; set; }
        public bool LdapLogin { get; set; }
        public string LdapUrl { get; set; }

        public bool Validate
        {
            get => !string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(Password) && !string.IsNullOrEmpty(Firstname) && !string.IsNullOrEmpty(Lastname) &&
                (!LdapLogin || !string.IsNullOrEmpty(LdapUrl));
        }
    }
}
