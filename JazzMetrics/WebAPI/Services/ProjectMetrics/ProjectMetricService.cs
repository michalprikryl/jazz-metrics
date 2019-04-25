using Database;
using Database.DAO;
using Library;
using Library.Models;
using Library.Models.ProjectMetricLogs;
using Library.Models.ProjectMetrics;
using Library.Models.ProjectMetricSnapshots;
using Library.Networking;
using Library.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Services.Helper;
using WebAPI.Services.Helpers;
using WebAPI.Services.ProjectMetricLogs;
using WebAPI.Services.ProjectMetricSnapshots;

namespace WebAPI.Services.ProjectMetrics
{
    public class ProjectMetricService : BaseDatabase, IProjectMetricService, IUser
    {
        private readonly IProjectMetricLogService _logService;
        private readonly IProjectMetricSnapshotService _snapshotService;

        public CurrentUser CurrentUser { get; set; }

        public ProjectMetricService(JazzMetricsContext db, IHelperService helperService, IHttpContextAccessor contextAccessor, IProjectMetricSnapshotService snapshotService,
            IProjectMetricLogService logService) : base(db)
        {
            _logService = logService;
            _snapshotService = snapshotService;
            CurrentUser = helperService.GetCurrentUser(contextAccessor.HttpContext.User.GetId());
        }

        public ProjectMetricModel ConvertToModel(ProjectMetric dbModel)
        {
            return new ProjectMetricModel
            {
                Id = dbModel.Id,
                CreateDate = dbModel.CreateDate,
                DataUrl = dbModel.DataUrl,
                MetricId = dbModel.MetricId,
                ProjectId = dbModel.ProjectId,
                Warning = dbModel.Warning,
                MinimalWarningValue = dbModel.MinimalWarningValue,
                LastUpdateDate = dbModel.LastUpdateDate,
                DataUsername = dbModel.DataUsername
            };
        }

        public async Task<BaseResponseModelPost> Create(ProjectMetricModel request)
        {
            BaseResponseModelPost response = new BaseResponseModelPost();

            if (request.Validate())
            {
                if (await CheckMetric(request, response) && await CheckProject(request.ProjectId, response) && TestURL(request.DataUrl, response))
                {
                    ProjectMetric projectMetric = new ProjectMetric
                    {
                        MetricId = request.MetricId,
                        ProjectId = request.ProjectId,
                        CreateDate = DateTime.Now,
                        LastUpdateDate = DateTime.Now,
                        DataUrl = request.DataUrl,
                        DataUsername = request.DataUsername,
                        DataPassword = PasswordHelper.Base64Encode(request.DataPassword),
                        Warning = request.Warning,
                        MinimalWarningValue = request.Warning ? request.MinimalWarningValue ?? 0 : default(decimal?)
                    };

                    await Database.ProjectMetric.AddAsync(projectMetric);

                    await Database.SaveChangesAsync();

                    response.Id = projectMetric.Id;
                    response.Message = "Project metric was successfully created!";
                }
            }
            else
            {
                response.Success = false;
                response.Message = "Some of the required properties is not present!";
            }

            return response;
        }

        public async Task<BaseResponseModel> Drop(int id)
        {
            BaseResponseModel response = new BaseResponseModel();

            ProjectMetric projectMetric = await Load(id, response);
            if (projectMetric != null)
            {
                foreach (var item in projectMetric.ProjectMetricSnapshot)
                {
                    Database.ProjectMetricColumnValue.RemoveRange(item.ProjectMetricColumnValue);
                }

                Database.ProjectMetricLog.RemoveRange(projectMetric.ProjectMetricLog);

                Database.ProjectMetricSnapshot.RemoveRange(projectMetric.ProjectMetricSnapshot);

                Database.ProjectMetric.Remove(projectMetric);

                await Database.SaveChangesAsync();

                response.Message = "Project metric was successfully deleted!";
            }

            return response;
        }

        public async Task<BaseResponseModel> Edit(ProjectMetricModel request)
        {
            BaseResponseModel response = new BaseResponseModel();

            if (request.Validate())
            {
                if (await CheckMetric(request, response) && await CheckProject(request.ProjectId, response) && TestURL(request.DataUrl, response))
                {
                    ProjectMetric projectMetric = await Load(request.Id, response);
                    if (projectMetric != null)
                    {
                        projectMetric.DataUrl = request.DataUrl;
                        projectMetric.DataUsername = request.DataUsername;
                        projectMetric.Warning = request.Warning;
                        projectMetric.MinimalWarningValue = request.Warning ? request.MinimalWarningValue ?? 0 : default(decimal?);

                        if (!string.IsNullOrEmpty(request.DataPassword))
                        {
                            projectMetric.DataPassword = PasswordHelper.Base64Encode(request.DataPassword);
                        }

                        await Database.SaveChangesAsync();

                        response.Message = "Project metric was successfully edited!";
                    }
                }
            }
            else
            {
                response.Success = false;
                response.Message = "Some of the required properties is not present!";
            }

            return response;
        }

        public async Task<BaseResponseModelGet<ProjectMetricModel>> Get(int id, bool lazy)
        {
            var response = new BaseResponseModelGet<ProjectMetricModel>();

            ProjectMetric projectMetric = await Load(id, response);
            if (projectMetric != null)
            {
                response.Value = ConvertToModel(projectMetric);

                if (!lazy)
                {
                    response.Value.Snapshots = GetSnapshots(projectMetric.ProjectMetricSnapshot);
                }
            }

            return response;
        }

        public Task<BaseResponseModelGetAll<ProjectMetricModel>> GetAll(bool lazy)
        {
            throw new NotImplementedException();
        }

        public async Task<BaseResponseModelGetAll<ProjectMetricLogModel>> GetProjectMetricLogs(int id)
        {
            var response = new BaseResponseModelGetAll<ProjectMetricLogModel>() { Values = new List<ProjectMetricLogModel>() };

            ProjectMetric projectMetric = await Load(id, response);
            if (projectMetric != null)
            {
                response.Values = GetLogs(projectMetric.ProjectMetricLog);
            }

            return response;
        }

        public async Task<ProjectMetric> Load(int id, BaseResponseModel response, bool tracking = true, bool lazy = true)
        {
            ProjectMetric projectMetric = await Database.ProjectMetric.FirstOrDefaultAsyncSpecial(a => a.Id == id, tracking, pm => pm.Metric);
            if (projectMetric == null)
            {
                response.Success = false;
                response.Message = "Unknown project metric!";
            }
            else
            {
                if (!projectMetric.Metric.Public && projectMetric.Metric.CompanyId != CurrentUser.CompanyId)
                {
                    projectMetric = null;
                    response.Success = false;
                    response.Message = "You don't have access to this project metric!";
                }
            }

            return projectMetric;
        }

        public Task<BaseResponseModel> PartialEdit(int id, List<PatchModel> request)
        {
            throw new NotImplementedException();
        }

        private List<ProjectMetricSnapshotModel> GetSnapshots(ICollection<ProjectMetricSnapshot> snapshots) => snapshots.Select(s => _snapshotService.ConvertToModel(s)).ToList();

        private List<ProjectMetricLogModel> GetLogs(ICollection<ProjectMetricLog> logs) => logs.Select(l => _logService.ConvertToModel(l)).ToList();

        private async Task<bool> CheckMetric(ProjectMetricModel projectMetric, BaseResponseModel response)
        {
            Metric metric = await Database.Metric.FirstOrDefaultAsync(a => a.Id == projectMetric.MetricId && (a.Public || (a.CompanyId.HasValue && a.CompanyId == CurrentUser.CompanyId)));
            if (metric != null)
            {
                if (projectMetric.Warning && metric.MetricType.NumberMetric)
                {
                    projectMetric.Warning = false;
                }

                return true;
            }
            else
            {
                response.Success = false;
                response.Message = "Unknown metric!";

                return false;
            }
        }

        private async Task<bool> CheckProject(int projectId, BaseResponseModel response)
        {
            if (await Database.Project.AnyAsync(a => a.Id == projectId))
            {
                return true;
            }
            else
            {
                response.Success = false;
                response.Message = "Unknown project!";

                return false;
            }
        }

        private bool TestURL(string url, BaseResponseModel response)
        {
            if(Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult) && (uriResult.Scheme == "http" || uriResult.Scheme == "https"))
            {
                return true;
            }
            else
            {
                response.Success = false;
                response.Message = "Bad URL format!";

                return false;
            }
        }
    }
}
