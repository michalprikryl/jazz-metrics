using Database.DAO;
using WebAPI.Models.Metric;
using WebAPI.Services.Helpers;

namespace WebAPI.Services.Metrics
{
    public interface IMetricService : ICrudOperations<MetricModel, Metric>
    {
    }
}
