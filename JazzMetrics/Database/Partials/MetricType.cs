namespace Database.DAO
{
    public partial class MetricType
    {
        public bool NumberMetric => Name.ToLower().Contains("number");
        public bool CoverageMetric => Name.ToLower().Contains("coverage");
    }
}
