using Library.Models;
using Library.Models.ProjectUsers;
using System.Threading.Tasks;

namespace WebApp.Services.Project
{
    public interface IProjectService
    {
        Task<BaseResponseModelGet<ProjectUserModel>> GetProjectUser(int userId, int projectId, string jwt);
    }
}
