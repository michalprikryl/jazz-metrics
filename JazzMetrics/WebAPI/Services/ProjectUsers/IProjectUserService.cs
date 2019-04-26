using Database.DAO;
using Library.Models;
using Library.Models.ProjectUsers;
using System.Threading.Tasks;
using WebAPI.Services.Helpers;

namespace WebAPI.Services.ProjectUsers
{
    /// <summary>
    /// interface pro servis pro praci s DB tabulkou ProjectUser
    /// </summary>
    public interface IProjectUserService : ICrudOperations<ProjectUserModel, ProjectUser>, IUser
    {
        /// <summary>
        /// vrati ProjectUser dle ID projektu a ID uzivatele
        /// </summary>
        /// <param name="projectId">ID projektu</param>
        /// <param name="userId">ID uzivatele</param>
        /// <returns></returns>
        Task<BaseResponseModelGet<ProjectUserModel>> GetByProjectAndUser(int projectId, int userId);
    }
}
