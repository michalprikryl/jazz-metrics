namespace Database.DAO
{
    public partial class MetricType
    {
        /// <summary>
        /// zda jde o metriku mnozstvi
        /// </summary>
        public bool NumberMetric => Name.ToLower().Contains("number");
        /// <summary>
        /// zda jde o metriku pokryti
        /// </summary>
        public bool CoverageMetric => Name.ToLower().Contains("coverage");
    }
}
