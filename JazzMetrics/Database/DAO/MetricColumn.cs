using System.Collections.Generic;

namespace Database.DAO
{
    public partial class MetricColumn
    {
        public MetricColumn()
        {
            ProjectMetricColumnValue = new HashSet<ProjectMetricColumnValue>();
        }

        public int Id { get; set; }
        public string Value { get; set; }
        public string FieldName { get; set; }
        public string NumberFieldName { get; set; }
        public string DivisorValue { get; set; }
        public string DivisorFieldName { get; set; }
        public int MetricId { get; set; }
        public string CoverageName { get; set; }

        public virtual Metric Metric { get; set; }
        public virtual ICollection<ProjectMetricColumnValue> ProjectMetricColumnValue { get; set; }
    }
}
