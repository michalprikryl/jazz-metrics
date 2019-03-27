using System;
using System.Collections.Generic;

namespace Database.DAO
{
    public partial class Metric
    {
        public Metric()
        {
            MetricColumn = new HashSet<MetricColumn>();
            ProjectMetric = new HashSet<ProjectMetric>();
        }

        public int Id { get; set; }
        public string Identificator { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string RequirementGroup { get; set; }
        public int MetricTypeId { get; set; }
        public int AspiceProcessId { get; set; }
        public int AffectedFieldId { get; set; }
        public int? CompanyId { get; set; }
        public bool Public { get; set; }

        public virtual AffectedField AffectedField { get; set; }
        public virtual AspiceProcess AspiceProcess { get; set; }
        public virtual Company Company { get; set; }
        public virtual MetricType MetricType { get; set; }
        public virtual ICollection<MetricColumn> MetricColumn { get; set; }
        public virtual ICollection<ProjectMetric> ProjectMetric { get; set; }
    }
}
