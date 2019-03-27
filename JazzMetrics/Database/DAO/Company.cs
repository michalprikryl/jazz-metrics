using System;
using System.Collections.Generic;

namespace Database.DAO
{
    public partial class Company
    {
        public Company()
        {
            Metric = new HashSet<Metric>();
            User = new HashSet<User>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Metric> Metric { get; set; }
        public virtual ICollection<User> User { get; set; }
    }
}
