using System;
using System.Collections.Generic;

namespace Database.DAO
{
    public partial class MetricType
    {
        public MetricType()
        {
            Metric = new HashSet<Metric>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Metric> Metric { get; set; }
    }
}
