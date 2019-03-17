using Database;
using Database.DAO;
using Library.Models;
using Library.Models.ProjectMetrics;
using Library.Models.ProjectMetricSnapshots;
using Library.Networking;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Services.Helper;
using WebAPI.Services.Helpers;
using WebAPI.Services.ProjectMetricSnapshots;

namespace WebAPI.Services.ProjectMetrics
{
    public class ProjectMetricService : BaseDatabase, IProjectMetricService, IUser
    {
        private readonly IProjectMetricSnapshotService _snapshotService;

        public CurrentUser CurrentUser { get; set; }

        public ProjectMetricService(JazzMetricsContext db, IHelperService helperService, IHttpContextAccessor contextAccessor, IProjectMetricSnapshotService snapshotService) : base(db)
        {
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
                if (await CheckMetric(request.MetricId, response) && await CheckProject(request.ProjectId, response))
                {
                    ProjectMetric projectMetric = new ProjectMetric
                    {
                        MetricId = request.MetricId,
                        ProjectId = request.ProjectId,
                        CreateDate = DateTime.Now,
                        LastUpdateDate = DateTime.Now,
                        DataUrl = request.DataUrl,
                        DataUsername = request.DataUsername,
                        DataPassword = PasswordHelper.EncodePassword(request.DataPassword, string.Empty),
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
                if (await CheckMetric(request.MetricId, response) && await CheckProject(request.ProjectId, response))
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
                            projectMetric.DataPassword = PasswordHelper.EncodePassword(request.DataPassword, string.Empty);
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

        public async Task<BaseResponseModelGetAll<ProjectMetricModel>> GetAllByProjectId(int projectId, bool lazy)
        {
            var response = new BaseResponseModelGetAll<ProjectMetricModel> { Values = new List<ProjectMetricModel>() };

            foreach (var item in await Database.ProjectMetric.Where(p => p.ProjectId == projectId).ToListAsync())
            {
                ProjectMetricModel projectMetric = ConvertToModel(item);

                if (!lazy)
                {
                    projectMetric.Snapshots = GetSnapshots(item.ProjectMetricSnapshot);
                }

                response.Values.Add(projectMetric);
            }

            return response;
        }

        public async Task<ProjectMetric> Load(int id, BaseResponseModel response)
        {
            ProjectMetric projectMetric = await Database.ProjectMetric.FirstOrDefaultAsync(a => a.Id == id);
            if (projectMetric == null)
            {
                response.Success = false;
                response.Message = "Unknown project metric!";
            }
            else
            {
                if (projectMetric.Metric.CompanyId != CurrentUser.CompanyId)
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

        private async Task<bool> CheckMetric(int metricId, BaseResponseModel response)
        {
            if (await Database.Metric.AnyAsync(a => a.Id == metricId && (a.Public || (a.CompanyId.HasValue && a.CompanyId == CurrentUser.CompanyId))))
            {
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
    }
}
