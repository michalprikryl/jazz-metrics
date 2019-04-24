namespace Library.Models
{
    /// <summary>
    /// model pro uchovani aktualne prihlaseneho uzivatele, ziskaneho z JWT
    /// </summary>
    public class CurrentUser
    {
        /// <summary>
        /// ID uzivatele
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// ID spolecnosti uzivatele
        /// </summary>
        public int? CompanyId { get; set; }
        /// <summary>
        /// nazev role uzivatele
        /// </summary>
        public string RoleName { get; set; }
    }
}
