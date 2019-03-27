using System;
using System.Collections.Generic;

namespace Database.DAO
{
    public partial class ProjectMetricLog
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public bool Warning { get; set; }
        public int ProjectMetricId { get; set; }

        public virtual ProjectMetric ProjectMetric { get; set; }
    }
}
