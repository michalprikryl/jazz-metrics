namespace Library.Models.Language
{
    /// <summary>
    /// model reprezentujici jazyk uzivatele
    /// </summary>
    public class LanguageModel
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// nazev jazyka
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// ISO639 1 kod (dvoumistny)
        /// </summary>
        public string Iso6391code { get; set; }
        /// <summary>
        /// ISO639 3 kod (trimistny)
        /// </summary>
        public string Iso6393code { get; set; }

        /// <summary>
        /// kontrola, zda jsou vyplnene povinne parametry
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $"{Name} ({Iso6391code})";
    }
}
