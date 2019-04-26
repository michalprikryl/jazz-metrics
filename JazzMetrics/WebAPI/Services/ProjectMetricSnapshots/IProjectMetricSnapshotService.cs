using Database.DAO;
using Library.Models.ProjectMetricSnapshots;
using WebAPI.Services.Helpers;

namespace WebAPI.Services.ProjectMetricSnapshots
{
    /// <summary>
    /// interface pro servis pro praci s DB tabulkou ProjectMetricSnapshot
    /// </summary>
    public interface IProjectMetricSnapshotService : ICrudOperations<ProjectMetricSnapshotModel, ProjectMetricSnapshot> { }
}
