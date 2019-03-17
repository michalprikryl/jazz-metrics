using Database.DAO;
using Library.Models.Metric;
using Library.Models.MetricColumn;
using WebAPI.Services.Helpers;

namespace WebAPI.Services.Metrics
{
    public interface IMetricService : ICrudOperations<MetricModel, Metric>
    {
        MetricColumnModel ConvertMetricColumn(MetricColumn metricColumn);
    }
}
