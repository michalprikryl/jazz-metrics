namespace Library.Models.User
{
    /// <summary>
    /// trida predstavujici navratovy objekt z API - pri identifikaci uzivatele
    /// </summary>
    public class UserIdentityModel
    {
        /// <summary>
        /// informace o uzivateli
        /// </summary>
        public UserCookieModel User { get; set; }
        /// <summary>
        /// JWT
        /// </summary>
        public string Token { get; set; }
    }

    /// <summary>
    /// objekt samotneho uzivatele - popis z API
    /// </summary>
    public class UserCookieModel
    {
        /// <summary>
        /// ID uzivatele
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// uzivatelske jmeno
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// jmeno uzivatele
        /// </summary>
        public string Firstname { get; set; }
        /// <summary>
        /// prijmeni uzivetele
        /// </summary>
        public string Lastname { get; set; }
        /// <summary>
        /// email uzivatele
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// nazev role uzivatele
        /// </summary>
        public string Role { get; set; }
        /// <summary>
        /// ID spolecnosti uzivatele
        /// </summary>
        public int? CompanyId { get; set; }

        /// <summary>
        /// reprezentace uzivatele ve formatu retezce
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $"{Firstname} {Lastname} ({Username}), email - {Email}, with user role '{Role}'{(CompanyId.HasValue ? $", from company #{CompanyId}" : string.Empty)}";
    }
}
