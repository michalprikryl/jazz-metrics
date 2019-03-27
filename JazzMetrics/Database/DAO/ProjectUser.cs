using System;
using System.Collections.Generic;

namespace Database.DAO
{
    public partial class ProjectUser
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int UserId { get; set; }
        public DateTime JoinDate { get; set; }

        public virtual Project Project { get; set; }
        public virtual User User { get; set; }
    }
}
