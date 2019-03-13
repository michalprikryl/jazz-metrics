using System.Collections.Generic;

namespace Database.DAO
{
    public partial class AspiceProcess
    {
        public AspiceProcess()
        {
            Metric = new HashSet<Metric>();
        }

        public int Id { get; set; }
        public string Shortcut { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int AspiceVersionId { get; set; }

        public virtual AspiceVersion AspiceVersion { get; set; }
        public virtual ICollection<Metric> Metric { get; set; }
    }
}
