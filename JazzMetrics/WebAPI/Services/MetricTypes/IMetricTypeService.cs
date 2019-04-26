using Database.DAO;
using Library.Models.MetricType;
using WebAPI.Services.Helpers;

namespace WebAPI.Services.MetricTypes
{
    /// <summary>
    /// interface pro servis pro praci s DB tabulkou MetricType
    /// </summary>
    public interface IMetricTypeService : ICrudOperations<MetricTypeModel, MetricType> { }
}
