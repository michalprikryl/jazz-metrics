using Database.DAO;
using Library.Models;
using Library.Models.ProjectMetrics;
using System.Threading.Tasks;
using WebAPI.Services.Helpers;

namespace WebAPI.Services.ProjectMetrics
{
    public interface IProjectMetricService : ICrudOperations<ProjectMetricModel, ProjectMetric>
    {
        Task<BaseResponseModelGetAll<ProjectMetricModel>> GetAllByProjectId(int projectId, bool lazy);
    }
}
