using System;

namespace Library.Models.ProjectMetricLogs
{
    public class ProjectMetricLogModel
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public bool Warning { get; set; }
        public int ProjectMetricId { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
