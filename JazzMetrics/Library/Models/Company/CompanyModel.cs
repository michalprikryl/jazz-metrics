using Library.Models.Users;
using System.Collections.Generic;

namespace Library.Models.Company
{
    /// <summary>
    /// model predstavujici spolecnost
    /// </summary>
    public class CompanyModel
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// nazev spolecnosti
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// seznam uzivatelu spolecnosti
        /// </summary>
        public List<UserModel> Users { get; set; }

        /// <summary>
        /// kontrola, zda jsou vyplnene povinne parametry
        /// </summary>
        /// <returns></returns>
        public bool Validate() => !string.IsNullOrEmpty(Name);
    }
}
