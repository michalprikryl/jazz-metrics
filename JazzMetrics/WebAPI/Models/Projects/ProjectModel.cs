using System;
using System.Collections.Generic;
using WebAPI.Models.ProjectMetrics;
using WebAPI.Models.Users;

namespace WebAPI.Models.Projects
{
    public class ProjectModel : BaseResponseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }

        public List<ProjectMetricModel> ProjectMetrics { get; set; }
        public List<UserModel> ProjectUsers { get; set; }

        public bool Validate
        {
            get => !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Description);
        }
    }
}
