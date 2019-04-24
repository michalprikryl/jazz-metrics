namespace Library.Models.AffectedFields
{
    /// <summary>
    /// model pro ovlivnenou oblast kvality vyvoje
    /// </summary>
    public class AffectedFieldModel
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// nazev oblasi
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// popis oblasti
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// kontrola, zda jsou vyplnene povinne parametry
        /// </summary>
        /// <returns></returns>
        public bool Validate() => !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Description);

        /// <summary>
        /// reprezentace ovlivnene oblasti
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{Name}";
        }
    }
}
