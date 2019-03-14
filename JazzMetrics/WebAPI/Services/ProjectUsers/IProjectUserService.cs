using Database.DAO;
using Library.Models;
using Library.Models.ProjectUsers;
using System.Threading.Tasks;
using WebAPI.Services.Helpers;

namespace WebAPI.Services.ProjectUsers
{
    public interface IProjectUserService : ICrudOperations<ProjectUserModel, ProjectUser>, IUser
    {
        Task<BaseResponseModelGet<ProjectUserModel>> GetByProjectAndUser(int projectId, int userId);
    }
}
