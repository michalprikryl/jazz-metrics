using Database.DAO;
using Library.Models.ProjectMetricLogs;
using WebAPI.Services.Helpers;

namespace WebAPI.Services.ProjectMetricLogs
{
    /// <summary>
    /// interface pro servis pro praci s DB tabulkou ProjectMetricLog
    /// </summary>
    public interface IProjectMetricLogService : ICrudOperations<ProjectMetricLogModel, ProjectMetricLog> { }
}
