using Database.DAO;
using Library.Models;
using Library.Models.Projects;
using System.Threading.Tasks;
using WebAPI.Services.Helpers;

namespace WebAPI.Services.Projects
{
    public interface IProjectService : ICrudOperations<ProjectModel, Project>, IUser
    {
        Task<BaseResponseModel> CreateSnapshots();
        Task<BaseResponseModel> CreateSnapshots(int id, Project project = null);
        Task<BaseResponseModel> CreateSnapshot(int id, int projectMetricId, ProjectMetric projectMetric = null);
    }
}
