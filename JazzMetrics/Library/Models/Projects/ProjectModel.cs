using Library.Models.ProjectMetrics;
using Library.Models.ProjectUsers;
using System;
using System.Collections.Generic;

namespace Library.Models.Projects
{
    public class ProjectModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }

        public List<ProjectMetricModel> ProjectMetrics { get; set; }
        public List<ProjectUserModel> ProjectUsers { get; set; }

        public bool Validate() => !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Description);
    }
}
