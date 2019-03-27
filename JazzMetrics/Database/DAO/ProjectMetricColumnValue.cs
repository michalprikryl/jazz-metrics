using System;
using System.Collections.Generic;

namespace Database.DAO
{
    public partial class ProjectMetricColumnValue
    {
        public int Id { get; set; }
        public decimal Value { get; set; }
        public int ProjectMetricSnapshotId { get; set; }
        public int MetricColumnId { get; set; }

        public virtual MetricColumn MetricColumn { get; set; }
        public virtual ProjectMetricSnapshot ProjectMetricSnapshot { get; set; }
    }
}
