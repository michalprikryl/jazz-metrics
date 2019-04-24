namespace Library.Models.MetricType
{
    /// <summary>
    /// model predstavujici typ metriky
    /// </summary>
    public class MetricTypeModel
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// nazev typu metriky
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// popis typu metriky
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// kontrola, zda jsou vyplnene povinne parametry
        /// </summary>
        /// <returns></returns>
        public bool Validate() => !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Description);

        /// <summary>
        /// zda jde o metriku mnozstvi
        /// </summary>
        public bool NumberMetric => Name.ToLower().Contains("number");
        /// <summary>
        /// zda jde o metriku pokryti
        /// </summary>
        public bool CoverageMetric => Name.ToLower().Contains("coverage");

        /// <summary>
        /// reprezentace typu metriky
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $"{Name}";
    }
}
