using System.Collections.Generic;

namespace Database.DAO
{
    public partial class MetricColumn
    {
        public MetricColumn()
        {
            InversePairMetricColumn = new HashSet<MetricColumn>();
            ProjectMetricColumnValue = new HashSet<ProjectMetricColumnValue>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool? Divisor { get; set; }
        public int? PairMetricColumnId { get; set; }
        public int MetricId { get; set; }

        public virtual Metric Metric { get; set; }
        public virtual MetricColumn PairMetricColumn { get; set; }
        public virtual ICollection<MetricColumn> InversePairMetricColumn { get; set; }
        public virtual ICollection<ProjectMetricColumnValue> ProjectMetricColumnValue { get; set; }
    }
}
