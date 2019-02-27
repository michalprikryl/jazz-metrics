using System;
using System.Collections.Generic;

namespace Database.DAO
{
    public partial class ProjectMetricSnapshot
    {
        public ProjectMetricSnapshot()
        {
            ProjectMetricColumnValue = new HashSet<ProjectMetricColumnValue>();
        }

        public int Id { get; set; }
        public DateTime InsertionDate { get; set; }
        public int ProjectMetricId { get; set; }

        public virtual ProjectMetric ProjectMetric { get; set; }
        public virtual ICollection<ProjectMetricColumnValue> ProjectMetricColumnValue { get; set; }
    }
}
