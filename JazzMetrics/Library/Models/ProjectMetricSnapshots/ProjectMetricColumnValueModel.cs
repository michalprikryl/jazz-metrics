using Library.Models.MetricColumn;

namespace Library.Models.ProjectMetricSnapshots
{
    public class ProjectMetricColumnValueModel
    {
        public int Id { get; set; }
        public decimal Value { get; set; }
        public int ProjectMetricSnapshotId { get; set; }
        public int MetricColumnId { get; set; }

        public virtual MetricColumnModel MetricColumn { get; set; }
    }
}
