using Database.DAO;
using WebAPI.Models.ProjectMetrics;
using WebAPI.Services.Helpers;

namespace WebAPI.Services.ProjectMetrics
{
    public interface IProjectMetricService : ICrudOperations<ProjectMetricModel, ProjectMetric>
    {
    }
}
