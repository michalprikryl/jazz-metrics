using System;
using System.Collections.Generic;

namespace WebApp.Models.Project
{
    public class ProjectListModel : ViewModel
    {
        public List<ProjectViewModel> Projects { get; set; }
    }

    public class ProjectViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
        public int ProjectMetricsCount { get; set; }
        public int ProjectUsersCount { get; set; }
    }
}
