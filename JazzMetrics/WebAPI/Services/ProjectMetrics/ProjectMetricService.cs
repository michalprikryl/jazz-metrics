using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Database;
using Database.DAO;
using Library.Networking;
using WebAPI.Models;
using WebAPI.Models.ProjectMetrics;
using WebAPI.Services.Helpers;

namespace WebAPI.Services.ProjectMetrics
{
    public class ProjectMetricService : BaseDatabase, IProjectMetricService
    {
        public ProjectMetricService(JazzMetricsContext db) : base(db)
        {
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

        public Task<BaseResponseModelPost> Create(ProjectMetricModel request)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponseModel> DropAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponseModel> Edit(ProjectMetricModel request)
        {
            throw new NotImplementedException();
        }

        public Task<ProjectMetricModel> Get(int id, bool lazy)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponseModelGet<ProjectMetricModel>> GetAll(bool lazy)
        {
            throw new NotImplementedException();
        }

        public Task<ProjectMetric> Load(int id, BaseResponseModel response)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponseModel> PartialEdit(int id, List<PatchModel> request)
        {
            throw new NotImplementedException();
        }
    }
}
