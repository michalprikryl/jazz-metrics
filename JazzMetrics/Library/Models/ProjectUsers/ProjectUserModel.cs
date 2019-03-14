using Library.Models.Users;
using System;

namespace Library.Models.ProjectUsers
{
    public class ProjectUserModel
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int UserId { get; set; }
        public DateTime JoinDate { get; set; }
        public UserModel User { get; set; }
    }
}
