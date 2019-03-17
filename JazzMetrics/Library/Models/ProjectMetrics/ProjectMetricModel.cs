using Library.Models.Metric;
using Library.Models.ProjectMetricSnapshots;
using Library.Models.Projects;
using System;
using System.Collections.Generic;

namespace Library.Models.ProjectMetrics
{
    public class ProjectMetricModel
    {
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

        public MetricModel Metric { get; set; }
        public ProjectModel Project { get; set; }

        public List<ProjectMetricSnapshotModel> Snapshots { get; set; }

        public bool Validate() => !string.IsNullOrEmpty(DataUrl) && !string.IsNullOrEmpty(DataUsername) && (DataPassword != null || DataPassword != string.Empty);
    }
}
