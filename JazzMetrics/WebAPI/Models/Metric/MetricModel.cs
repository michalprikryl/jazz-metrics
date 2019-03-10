using WebAPI.Models.AffectedFields;
using WebAPI.Models.AspiceProcesses;
using WebAPI.Models.MetricType;

namespace WebAPI.Models.Metric
{
    public class MetricModel : BaseResponseModel
    {
        public int Id { get; set; }
        public string Identificator { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int MetricTypeId { get; set; }
        public int AspiceProcessId { get; set; }
        public int AffectedFieldId { get; set; }

        public MetricTypeModel MetricType { get; set; }
        public AffectedFieldModel AffectedField { get; set; }
        public AspiceProcessModel AspiceProcess { get; set; }

        public bool Validate
        {
            get => !string.IsNullOrEmpty(Identificator) && !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Description);
        }
    }
}
