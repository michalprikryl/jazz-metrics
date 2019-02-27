using System;
using System.Collections.Generic;

namespace Database.DAO
{
    public partial class AspiceVersion
    {
        public AspiceVersion()
        {
            AspiceProcess = new HashSet<AspiceProcess>();
        }

        public int Id { get; set; }
        public decimal VersionNumber { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Description { get; set; }

        public virtual ICollection<AspiceProcess> AspiceProcess { get; set; }
    }
}
