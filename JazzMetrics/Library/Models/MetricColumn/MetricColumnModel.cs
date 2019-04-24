namespace Library.Models.MetricColumn
{
    /// <summary>
    /// model predstavujici atribut metriky (sloupec)
    /// </summary>
    public class MetricColumnModel
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// hodnota tagu v XML
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// nazev tagu v XML -> vaze k Value
        /// </summary>
        public string FieldName { get; set; }
        /// <summary>
        /// nazev tagu v XML s ciselnou hodnotou
        /// </summary>
        public string NumberFieldName { get; set; }
        /// <summary>
        /// hodnota tagu v XML pro delitele metriky pokryti
        /// </summary>
        public string DivisorValue { get; set; }
        /// <summary>
        /// nazev tagu v XML delitele metriky pokryti
        /// </summary>
        public string DivisorFieldName { get; set; }
        /// <summary>
        /// ID metriky
        /// </summary>
        public int MetricId { get; set; }
        /// <summary>
        /// nazev pokryti
        /// </summary>
        public string CoverageName { get; set; }

        /// <summary>
        /// kontrola, zda jsou vyplnene povinne parametry
        /// </summary>
        /// <returns></returns>
        public bool Validate() =>
            (string.IsNullOrEmpty(CoverageName) && !string.IsNullOrEmpty(FieldName) && !string.IsNullOrEmpty(NumberFieldName)) //number column
            || (!string.IsNullOrEmpty(CoverageName)  && !string.IsNullOrEmpty(FieldName)); //coverage column
    }
}
