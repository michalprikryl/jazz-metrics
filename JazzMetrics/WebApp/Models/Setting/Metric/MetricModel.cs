using WebApp.Models.Setting.AffectedField;
using WebApp.Models.Setting.AspiceProcess;
using WebApp.Models.Setting.MetricType;

namespace WebApp.Models.Setting.Metric
{
    public class MetricModel : BaseApiResult
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
    }
}
