namespace Library.Models.Users
{
    /// <summary>
    /// model predstavujici uzivatele
    /// </summary>
    public class UserModel
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// prihlasovaci jmeno
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// email uzivatele
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// jmeno uzivatele
        /// </summary>
        public string Firstname { get; set; }
        /// <summary>
        /// prijmeni uzivatele
        /// </summary>
        public string Lastname { get; set; }
        /// <summary>
        /// preferovany jazyk uzivatele
        /// </summary>
        public int LanguageId { get; set; }
        /// <summary>
        /// ID role uzivatele
        /// </summary>
        public int UserRoleId { get; set; }
        /// <summary>
        /// ID spolecnosti uzivatele
        /// </summary>
        public int? CompanyId { get; set; }
        /// <summary>
        /// true -> pouziva se LDAP prihlaseni (neni hotove)
        /// </summary>
        public bool UseLdaplogin { get; set; }
        /// <summary>
        /// URL LDAP
        /// </summary>
        public string LdapUrl { get; set; }
        /// <summary>
        /// true -> uzivatel je admin
        /// </summary>
        public bool Admin { get; set; }

        /// <summary>
        /// heslo uzivatele
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// pro specialni potreby vytvoreni uzivatele a firmy najednou
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        /// kontrola, zda jsou vyplnene povinne parametry
        /// </summary>
        /// <returns></returns>
        public bool Validate() => !string.IsNullOrEmpty(Password) && ValidateEdit();

        /// <summary>
        /// kontrola, zda jsou vyplnene povinne parametry -> pro editaci
        /// </summary>
        /// <returns></returns>
        public bool ValidateEdit() => !string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(Firstname) && !string.IsNullOrEmpty(Lastname) && (!UseLdaplogin || !string.IsNullOrEmpty(LdapUrl));

        /// <summary>
        /// reprezentace uzivatele ve formatu retezce
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $"{Firstname} {Lastname} ({Email})";
    }
}
