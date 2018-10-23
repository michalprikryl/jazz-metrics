namespace WebAPI.Models.User
{
    /// <summary>
    /// trida pro kontext uzivatele ve scanneru
    /// </summary>
    public class UserModel
    {
        /// <summary>
        /// ID uzivatele z DB
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// jmeno uzivatele
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// prijmeni uzivatele
        /// </summary>
        public string Lastname { get; set; }
        /// <summary>
        /// username uzivatele
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// role uzivatele
        /// </summary>
        public string Role { get; set; }
    }
}