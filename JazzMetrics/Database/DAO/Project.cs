using System;
using System.Collections.Generic;

namespace Database.DAO
{
    public partial class Project
    {
        public Project()
        {
            ProjectMetric = new HashSet<ProjectMetric>();
            ProjectUser = new HashSet<ProjectUser>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }

        public virtual ICollection<ProjectMetric> ProjectMetric { get; set; }
        public virtual ICollection<ProjectUser> ProjectUser { get; set; }
    }
}
