using Database.DAO;
using Library.Models.MetricType;
using WebAPI.Services.Helpers;

namespace WebAPI.Services.MetricTypes
{
    public interface IMetricTypeService : ICrudOperations<MetricTypeModel, MetricType> { }
}
