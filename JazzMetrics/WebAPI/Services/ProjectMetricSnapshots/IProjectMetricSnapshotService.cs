using Database.DAO;
using Library.Models.ProjectMetricSnapshots;
using WebAPI.Services.Helpers;

namespace WebAPI.Services.ProjectMetricSnapshots
{
    public interface IProjectMetricSnapshotService : ICrudOperations<ProjectMetricSnapshotModel, ProjectMetricSnapshot>
    {
    }
}
