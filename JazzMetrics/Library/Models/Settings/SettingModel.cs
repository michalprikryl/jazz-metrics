namespace Library.Models.Settings
{
    /// <summary>
    /// model predstavujici nastaveni
    /// </summary>
    public class SettingModel
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// ramec/skupina nastaveni
        /// </summary>
        public string SettingScope { get; set; }
        /// <summary>
        /// nazev nastaveni v ramci skupiny
        /// </summary>
        public string SettingName { get; set; }
        /// <summary>
        /// hodnota nastaveni
        /// </summary>
        public string Value { get; set; }
    }
}
