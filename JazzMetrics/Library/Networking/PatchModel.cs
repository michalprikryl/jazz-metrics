namespace Library.Networking
{
    /// <summary>
    /// model pro HTTP PATCH
    /// </summary>
    public class PatchModel
    {
        /// <summary>
        /// nazev vlastnosti entity ke zmene
        /// </summary>
        public string PropertyName { get; set; }
        /// <summary>
        /// hodnota vlastnosti entity k nahrazeni
        /// </summary>
        public string Value { get; set; }
    }
}
