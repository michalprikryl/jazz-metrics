using System;
using System.Collections.Generic;
using WebApp.Models.Project.ProjectMetric;
using WebApp.Models.User;

namespace WebApp.Models.Project
{
    public class ProjectModel : BaseApiResult
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }

        public List<ProjectMetricModel> ProjectMetrics { get; set; }
        public List<UserModel> ProjectUsers { get; set; }
    }
}
