namespace Library.Models.MetricColumn
{
    public class MetricColumnModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int MetricId { get; set; }
        public int? PairMetricColumnId { get; set; }

        public bool Validate() => !string.IsNullOrEmpty(Name);
    }
}
