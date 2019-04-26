using Database.DAO;
using Library.Models.Metric;
using Library.Models.MetricColumn;
using System.Collections.Generic;
using WebAPI.Services.Helpers;

namespace WebAPI.Services.Metrics
{
    /// <summary>
    /// interface pro servis pro praci s DB tabulkou Metrics
    /// </summary>
    public interface IMetricService : ICrudOperations<MetricModel, Metric>
    {
        /// <summary>
        /// prevede entitu z databaze (sloupec metriky) na model
        /// </summary>
        /// <param name="metricColumn">entita z DB</param>
        /// <returns></returns>
        MetricColumnModel ConvertMetricColumn(MetricColumn metricColumn);
        /// <summary>
        /// prevede seznam entit z databaze (sloupec metriky) na model
        /// </summary>
        /// <param name="metricColumns">entity z DB</param>
        /// <returns></returns>
        List<MetricColumnModel> GetMetricColumns(ICollection<MetricColumn> metricColumns);
    }
}
