using System;
using WebAPI.Models.Metric;
using WebAPI.Models.Projects;

namespace WebAPI.Models.ProjectMetrics
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
    }
}
