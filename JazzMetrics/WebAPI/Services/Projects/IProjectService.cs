using Database.DAO;
using Library.Models;
using Library.Models.Projects;
using System.Threading.Tasks;
using WebAPI.Services.Helpers;

namespace WebAPI.Services.Projects
{
    /// <summary>
    /// interface pro servis pro praci s DB tabulkou Project
    /// </summary>
    public interface IProjectService : ICrudOperations<ProjectModel, Project>, IUser
    {
        /// <summary>
        /// pro ziskani dat na projektovou nastenku
        /// </summary>
        /// <param name="id">ID projektu</param>
        /// <returns></returns>
        Task<BaseResponseModelGet<ProjectModel>> Get(int id);
        /// <summary>
        /// stahne data a vytvori snapshoty pro vsechny projektove metriky vsech projektu
        /// </summary>
        /// <returns></returns>
        Task<BaseResponseModel> CreateSnapshots();
        /// <summary>
        /// stahne data a vytvori snapshoty pro vsechny projektove metriky projektu
        /// </summary>
        /// <param name="id">ID projektu</param>
        /// <param name="project">projekt</param>
        /// <returns></returns>
        Task<BaseResponseModel> CreateSnapshots(int id, Project project = null);
        /// <summary>
        /// stahne data a vytvori snapshoty pro projektovou metriku
        /// </summary>
        /// <param name="id">ID projektu</param>
        /// <param name="projectMetricId">ID projektove metriky</param>
        /// <param name="projectMetric">projektova metrika</param>
        /// <returns></returns>
        Task<BaseResponseModel> CreateSnapshot(int id, int projectMetricId, ProjectMetric projectMetric = null);
    }
}
