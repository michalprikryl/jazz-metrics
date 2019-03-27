using System;
using System.Collections.Generic;

namespace Database.DAO
{
    public partial class ProjectMetric
    {
        public ProjectMetric()
        {
            ProjectMetricLog = new HashSet<ProjectMetricLog>();
            ProjectMetricSnapshot = new HashSet<ProjectMetricSnapshot>();
        }

        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int MetricId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public string DataUrl { get; set; }
        public string DataUsername { get; set; }
        public string DataPassword { get; set; }
        public bool Warning { get; set; }
        public decimal? MinimalWarningValue { get; set; }

        public virtual Metric Metric { get; set; }
        public virtual Project Project { get; set; }
        public virtual ICollection<ProjectMetricLog> ProjectMetricLog { get; set; }
        public virtual ICollection<ProjectMetricSnapshot> ProjectMetricSnapshot { get; set; }
    }
}
