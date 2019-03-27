using Library.Models.AffectedFields;
using Library.Models.AspiceProcesses;
using Library.Models.MetricColumn;
using Library.Models.MetricType;
using System.Collections.Generic;

namespace Library.Models.Metric
{
    public class MetricModel
    {
        public int Id { get; set; }
        public string Identificator { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string RequirementGroup { get; set; }
        public int MetricTypeId { get; set; }
        public int AspiceProcessId { get; set; }
        public int AffectedFieldId { get; set; }
        public bool Public { get; set; }
        public int? CompanyId { get; set; }

        public MetricTypeModel MetricType { get; set; }
        public AffectedFieldModel AffectedField { get; set; }
        public AspiceProcessModel AspiceProcess { get; set; }

        public List<MetricColumnModel> Columns { get; set; }

        public bool Validate() => !string.IsNullOrEmpty(Identificator) && !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Description) && !string.IsNullOrEmpty(RequirementGroup);

        public override string ToString() => $"{Name} ({Identificator})";
    }
}
