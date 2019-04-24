using Library.Models.AspiceVersions;

namespace Library.Models.AspiceProcesses
{
    /// <summary>
    /// model popisujici proces ASPICE
    /// </summary>
    public class AspiceProcessModel
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// nazev procesu
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// zkratka procesu (napr. SWE.1)
        /// </summary>
        public string Shortcut { get; set; }
        /// <summary>
        /// popis procesu
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// ID verze ASPICE, ze ktere proces pochazi
        /// </summary>
        public int AspiceVersionId { get; set; }
        /// <summary>
        /// odkaz na verzi ASPICE, ze ktere proces pochazi
        /// </summary>
        public AspiceVersionModel AspiceVersion { get; set; }

        /// <summary>
        /// kontrola, zda jsou vyplnene povinne parametry
        /// </summary>
        /// <returns></returns>
        public bool Validate() => !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Shortcut) && !string.IsNullOrEmpty(Description);

        /// <summary>
        /// reprezentace procesu
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $"{Name} ({Shortcut})";
    }
}
