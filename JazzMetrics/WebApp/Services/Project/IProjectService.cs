using Library.Models;
using Library.Models.ProjectMetricLogs;
using Library.Models.Projects;
using Library.Models.ProjectUsers;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.Services.Project
{
    public interface IProjectService
    {
        Task<BaseResponseModel> UpdateProjectMetric(int projectId, int projectMetricId, string jwt);
        Task<BaseResponseModel> UpdateAllProjectMetrics(int projectId, string jwt);
        Task<BaseResponseModelGet<ProjectModel>> GetFullProject(int projectId, string jwt);
        Task<List<SelectListItem>> GetMetricsForSelect(string jwt);
        Task<BaseResponseModelGet<ProjectUserModel>> GetProjectUser(int userId, int projectId, string jwt);
        Task<BaseResponseModelGetAll<ProjectMetricLogModel>> GetProjectMetricLog(int projectMetricId, string jwt);
    }
}
