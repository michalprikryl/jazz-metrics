using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models.Project.ProjectUser
{
    public class ProjectUserListModel : ViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ProjectUserWorkModel User { get; set; }
        public List<ProjectUserViewModel> Users { get; set; }
    }

    public class ProjectUserWorkModel
    {
        public int ProjectId { get; set; }

        [Required(ErrorMessage = "Username is required!")]
        public string Username { get; set; }
    }

    public class ProjectUserViewModel
    {
        public int UserId { get; set; }
        public bool Admin { get; set; }
        public string Username { get; set; }
        public string UserInfo { get; set; }
        public DateTime JoinDate { get; set; }
    }
}
