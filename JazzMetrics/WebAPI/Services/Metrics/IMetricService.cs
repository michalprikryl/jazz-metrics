using Database.DAO;
using Library.Models.Metric;
using WebAPI.Services.Helpers;

namespace WebAPI.Services.Metrics
{
    public interface IMetricService : ICrudOperations<MetricModel, Metric>
    {
    }
}
