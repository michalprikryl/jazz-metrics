using Library.Models;
using Library.Models.ProjectUsers;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.Services.Project
{
    public interface IProjectService
    {
        Task<List<SelectListItem>> GetMetricsForSelect(string jwt);
        Task<BaseResponseModelGet<ProjectUserModel>> GetProjectUser(int userId, int projectId, string jwt);
    }
}
