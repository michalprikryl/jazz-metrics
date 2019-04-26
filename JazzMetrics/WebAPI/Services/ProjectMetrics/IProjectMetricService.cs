using Database.DAO;
using Library.Models;
using Library.Models.ProjectMetricLogs;
using Library.Models.ProjectMetrics;
using System.Threading.Tasks;
using WebAPI.Services.Helpers;

namespace WebAPI.Services.ProjectMetrics
{
    /// <summary>
    /// interface pro servis pro praci s DB tabulkou ProjectMetrics
    /// </summary>
    public interface IProjectMetricService : ICrudOperations<ProjectMetricModel, ProjectMetric>
    {
        /// <summary>
        /// vrati kompletni log projektove metriky
        /// </summary>
        /// <param name="id">ID projektove metriky</param>
        /// <returns></returns>
        Task<BaseResponseModelGetAll<ProjectMetricLogModel>> GetProjectMetricLogs(int id);
    }
}
