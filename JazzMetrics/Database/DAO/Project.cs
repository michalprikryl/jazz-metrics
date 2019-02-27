using System;
using System.Collections.Generic;

namespace Database.DAO
{
    public partial class Project
    {
        public Project()
        {
            ProjectMetric = new HashSet<ProjectMetric>();
            UserProject = new HashSet<UserProject>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }

        public virtual ICollection<ProjectMetric> ProjectMetric { get; set; }
        public virtual ICollection<UserProject> UserProject { get; set; }
    }
}
