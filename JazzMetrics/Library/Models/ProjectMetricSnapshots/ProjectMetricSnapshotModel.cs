using System;
using System.Collections.Generic;

namespace Library.Models.ProjectMetricSnapshots
{
    public class ProjectMetricSnapshotModel
    {
        public int Id { get; set; }
        public DateTime InsertionDate { get; set; }
        public int ProjectMetricId { get; set; }

        public List<ProjectMetricColumnValueModel> Values { get; set; }
    }
}
