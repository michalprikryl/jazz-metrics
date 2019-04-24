using System;

namespace Library.Models.AspiceVersions
{
    /// <summary>
    /// model verze ASPICE
    /// </summary>
    public class AspiceVersionModel
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// cislo verze
        /// </summary>
        public decimal VersionNumber { get; set; }
        /// <summary>
        /// datum zverejneni verze
        /// </summary>
        public DateTime ReleaseDate { get; set; }
        /// <summary>
        /// popis verze
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// kontrola, zda jsou vyplnene povinne parametry
        /// </summary>
        /// <returns></returns>
        public bool Validate() => VersionNumber > 0 && ReleaseDate > DateTime.Now.AddYears(-20) && !string.IsNullOrEmpty(Description);

        /// <summary>
        /// reprezentace standardu
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $"Version {VersionNumber}";
    }
}
