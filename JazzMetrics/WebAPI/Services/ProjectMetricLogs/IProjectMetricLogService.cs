using Database.DAO;
using Library.Models.ProjectMetricLogs;
using WebAPI.Services.Helpers;

namespace WebAPI.Services.ProjectMetricLogs
{
    public interface IProjectMetricLogService : ICrudOperations<ProjectMetricLogModel, ProjectMetricLog> { }
}
